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
}
