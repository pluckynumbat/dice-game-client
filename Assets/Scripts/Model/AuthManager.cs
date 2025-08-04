using System;
using System.Threading.Tasks;
using Network;
using Presentation.Loading.Screen;
using UnityEngine;
using UnityEngine.Networking;

namespace Model
{
    /// <summary>
    /// This manager performs authentication flows, and stores related data like session id etc
    /// </summary>
    public class AuthManager
    {
        private const int AuthRetryCount = 4;
        
        public string PlayerID;
        public string SessionID;
        
        public async Task<AuthLoginData> RequestLogin(string authHeaderPayload, bool isNewUser, string serverVersion)
        {
            LoginRequest request = new LoginRequest();
            AuthLoginData loginData = await request.Send(authHeaderPayload, isNewUser, serverVersion);

            if (!string.IsNullOrEmpty(loginData.loginResponse.playerID))
            {
                PlayerID = loginData.loginResponse.playerID;
                SessionID = loginData.sessionID;
            }
            else
            {
                Debug.LogError("could not login :(");
            }

            return loginData;
        }
        
        public async Task RequestLogout()
        {
            if (SessionID == null)
            {
                Debug.LogWarning("cannot logout, the session ID is null");
            }

            LogoutRequest request = new LogoutRequest();
            bool success = await request.Send(SessionID);

            if (success)
            {
                PlayerID = null;
                SessionID = null;
            }
            else
            {
                Debug.LogWarning("could not logout :(");
            }
        }
        
        // This will orchestrate the various steps involved in the auth flow, along with updating the loading screen bar
        public async Task PerformAuthFlow(LoadingScreen loadingScreen)
        {
            // change the game state, getting the loading bar started
            GameRoot.Instance.StateManager.ChangeGameState(StateManager.GameState.Auth);
            if (loadingScreen != null)
            {
                await loadingScreen.ShowProgress(.1f, 0.1f);
            }

            // core task: start basic authentication task and await on results
            BasicAuthTaskResult basicAuthTaskResult = await BasicAuthenticationTask();
            if (!basicAuthTaskResult.Success)
            {
                GameRoot.Instance.ErrorManager.EnterErrorState(ErrorType.CriticalError);
                Debug.LogError("Basic auth task failed");
                return;
            }

            if (loadingScreen != null) // let the loading bar go to 50%
            {
                await loadingScreen.ShowProgress(.5f, 0.4f);
            }

            // on successful auth, fetch the config and player data concurrently
            Task<bool> configTask = FetchConfigTask();
            Task<bool> playerDataTask = FetchPlayerDataTask(PlayerID, basicAuthTaskResult.IsNewUser);
            
            if (loadingScreen != null)  // let the loading bar go to 75%
            {
                await loadingScreen.ShowProgress(.7f, 0.2f);
            }

            await Task.WhenAll(configTask, playerDataTask);
            
            if (loadingScreen != null) // let the loading bar go to 90%
            {
                await loadingScreen.ShowProgress(.9f, 0.2f);
            }

            // early out if either of the above 2 tasks fail
            // we will already be in the critical error state based on their requests
            if (!configTask.Result)
            {
                Debug.LogError("Fetch config task failed");
                return;
            }
            
            if (!playerDataTask.Result)
            {
                Debug.LogError("Fetch player data / create new player task failed");
                return;
            }

            // fetch the player stats
            await FetchStatsTask(PlayerID);
            if (loadingScreen != null) // let the loading bar go to 90%
            {
                await loadingScreen.ShowProgress(1.0f, 0.1f);
            }
            
            // finally, change the state to main menu
            GameRoot.Instance.StateManager.ChangeGameState(StateManager.GameState.MainMenu);
        }

        private async Task<BasicAuthTaskResult> BasicAuthenticationTask()
        {
            // 1. Check if we have a stored guid, server version in player prefs
            string existingGuid = PlayerPrefs.GetString("player-guid", null);
            string serverVersion = PlayerPrefs.GetString("server-version", null);
            
            // NOTE: isNewUser will just be a suggestion to the auth server. If the server was (re)started after our user was created, 
            // (serverVersion mismatch), this user will still be treated as a new user from the perspective of the auth server
            bool isNewUserRequest = string.IsNullOrEmpty(existingGuid);
            string requestServerVersion = !string.IsNullOrEmpty(serverVersion) ? serverVersion : "0";
            
            // 2. generate a new guid if we need to, or use the existing one
            string authGuidToUse = isNewUserRequest ? Guid.NewGuid().ToString() : existingGuid;
            
            // 3. create the 'basic authorization' header payload from our authGuidToUse (used as both username and password)
            string authHeaderPayload = CreateBasicAuthHeaderPayload(authGuidToUse, authGuidToUse);
               
            // 4. send the login request and await the response
            // retry a few times if it is a connection error (could not connect / server timeout) before marking failure
            AuthLoginData loginData = new AuthLoginData(){ loginResponse = new LoginResponse(), loginResult = UnityWebRequest.Result.InProgress, sessionID = null};
            int attemptCount = AuthRetryCount + 1;
            while (attemptCount > 0)
            {
                loginData = await RequestLogin(authHeaderPayload, isNewUserRequest, requestServerVersion);
                if (!string.IsNullOrEmpty(loginData.loginResponse.playerID) || loginData.loginResult != UnityWebRequest.Result.ConnectionError)
                {
                    break;
                }
                Debug.LogWarning("auth login request could not connect to the server, retrying");
                --attemptCount;
                await Task.Delay(NetRequestManager.RetryDelayMilliseconds);
            }
            
            // 5. write values to the task result (and player prefs) based on the response
            BasicAuthTaskResult result;
            result.Success = !string.IsNullOrEmpty(loginData.loginResponse.playerID);
            result.IsNewUser = isNewUserRequest;
            
            if (result.Success)
            {
                PlayerPrefs.SetString("player-guid", authGuidToUse);
                PlayerPrefs.SetString("server-version", loginData.loginResponse.serverVersion);
            }
            
            // 5. return the result!
            return result;
        }

        private async Task<bool> FetchConfigTask()
        {
           await GameRoot.Instance.ConfigManager.RequestConfig(new RequestParams() 
               { Timeout = 10, Retries = 1, ErrorOnFail = ErrorType.CriticalError}); // go into critical error state if the request fails
            return GameRoot.Instance.ConfigManager.GameConfig.levels != null;
        }
        
        // depending on new user VS existing user, the following task has different flows
        private async Task<bool> FetchPlayerDataTask(string playerID, bool isNewPlayerWrtClient)
        {
            // in this case (client thinks it is a new user), the auth server cannot consider the client to be an
            // existing user (actual server version will never be 0), so we are good to proceed as a new player
            if (isNewPlayerWrtClient)
            {
                 await GameRoot.Instance.PlayerManager.RequestNewPlayerCreation(playerID, new RequestParams() 
                     { Timeout = 10, Retries = 1, ErrorOnFail = ErrorType.CriticalError}); // go into critical error state if the request fails
            }
            else // the client considers itself an existing user
            {
                // the auth server might consider the player to be new or existing based on its own server version. In either case,
                // this status actually depends on whether there is player data on the data server for this player, which there should be
                // unless the data server was restarted since the last time the player logged in. So let's check by sending the Get Player request:
                // if it succeeds, we proceed as an existing player. If it fails with an http status 404, that is fine, and we will proceed as a new player
                await GameRoot.Instance.PlayerManager.RequestPlayerData(playerID, new RequestParams()
                    { Timeout = 10, Retries = 1, ErrorOnFail = ErrorType.CriticalError, IsNotFoundOk = true}); // any fail status other than 404 should go into critical error state
                   
                // ^if the above request failed with a 404 (there was no data stored that can be recovered), proceed as a new player
                if (string.IsNullOrEmpty(GameRoot.Instance.PlayerManager.PlayerData.playerID))
                {
                    await GameRoot.Instance.PlayerManager.RequestNewPlayerCreation(playerID, new RequestParams()
                        { Timeout = 10, Retries = 1, ErrorOnFail = ErrorType.CriticalError}); // go into critical error state if the request fails
                }
            }
            
            return !string.IsNullOrEmpty(GameRoot.Instance.PlayerManager.PlayerData.playerID);
        }
        
        private async Task FetchStatsTask(string playerID)
        {
            await GameRoot.Instance.PlayerManager.RequestPlayerStats(playerID,  new RequestParams() { Timeout = 5, Retries = 0 }); // no error state if this fails
        }

        // creates a base64 encoding of the string <username:password> and adds a prefix "Basic " to it
        // reference: https://en.wikipedia.org/wiki/Basic_access_authentication
        public string CreateBasicAuthHeaderPayload(string username, string password)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes( username + ":" + password);
            return "Basic " + Convert.ToBase64String(bytes);
        }
    }

    struct BasicAuthTaskResult
    {
        public bool Success;
        public bool IsNewUser; // this is if the client believes itself to be a new user
    }
}
