using System;
using System.Threading.Tasks;
using Network;
using Presentation.Loading.Screen;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// This manager performs authentication flows, and stores related data like session id etc
    /// </summary>
    public class AuthManager
    {
        public string PlayerID;
        public string SessionID;
        
        public async Task<AuthLoginData> RequestLogin(string authHeaderPayload, bool isNewUser)
        {
            LoginRequest request = new LoginRequest();
            AuthLoginData loginData = await request.Send(authHeaderPayload, isNewUser);

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
            LogoutRequest request = new LogoutRequest();
            bool success = await request.Send(SessionID);

            if (success)
            {
                PlayerID = null;
                SessionID = null;
            }
            else
            {
                Debug.LogError("could not logout :(");
            }
        }
        
        private async Task<BasicAuthTaskResult> BasicAuthenticationTask()
        {
            BasicAuthTaskResult result;
            
            // 1. Check if we have a stored guid in player prefs
            string existingGuid = PlayerPrefs.GetString("player-guid", null);
            
            // TODO: we should also check if the server was started after our guid was created because 
            // in that case even if we have a stored guid, it won't be of any use to us as the server
            // does not know about it... so we need to do new user login in that case
            result.IsNewUser = string.IsNullOrEmpty(existingGuid);

            // 2. generate a new guid if we need to, or use the existing one
            string authGuidToUse = result.IsNewUser ? Guid.NewGuid().ToString() : existingGuid;
            
            // 3. create the 'basic authorization' header payload from our authGuidToUse (used as both username and password)
            string authHeaderPayload = CreateBasicAuthHeaderPayload(authGuidToUse, authGuidToUse);
               
            // 4. send the login request and await the response
            AuthLoginData loginData = await RequestLogin(authHeaderPayload, result.IsNewUser);
            result.Success = !string.IsNullOrEmpty(loginData.loginResponse.playerID);
            if (result.Success)
            {
                if (result.IsNewUser)
                {
                    PlayerPrefs.SetString("player-guid", authGuidToUse);
                }
            }
            else
            {
                // TODO: error handling
                Debug.LogError("login request failed");
            }
            
            // 5. return the result!
            return result;
        }

        private async Task<bool> FetchConfigTask()
        {
            await GameRoot.Instance.ConfigManager.RequestConfig();
            return GameRoot.Instance.ConfigManager.GameConfig.levels != null;
        }
        
        private async Task<bool> FetchPlayerDataTask(string playerID, bool isNewPlayer)
        {
            if (isNewPlayer)
            {
                await GameRoot.Instance.PlayerManager.RequestNewPlayerCreation(playerID);
            }
            else
            {
                await GameRoot.Instance.PlayerManager.RequestPlayerData(playerID);
            }
            
            return !string.IsNullOrEmpty(GameRoot.Instance.PlayerManager.PlayerData.playerID);
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
        public bool IsNewUser;
    }
}
