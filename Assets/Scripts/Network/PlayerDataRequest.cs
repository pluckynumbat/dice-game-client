using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    /// The network request to get the data of an existing player
    /// </summary>
    public class PlayerDataRequest
    {
        private const string ServerHost = "http://localhost";
        private const string ServerPort = "8080";
        private const string Endpoint = "/profile/player-data/";
    
        public async Task<PlayerData> Send(string sessionID, string playerID, int timeout = 30)
        {
            PlayerData playerData;
        
            string uri = $"{ServerHost}:{ServerPort}{Endpoint}{playerID}";
        
            UnityWebRequest getRequest = UnityWebRequest.Get(uri);
            getRequest.timeout = timeout;
        
            getRequest.SetRequestHeader("Session-Id", sessionID);

            await getRequest.SendWebRequest();
        
            switch (getRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log($"success, player data response: {getRequest.downloadHandler.text}");
                    playerData = JsonUtility.FromJson<PlayerData>(getRequest.downloadHandler.text);
                    break;

                default:
                    Debug.LogError($"failure, reason: {getRequest.error}");
                    playerData = default(PlayerData);
                    break;
            }
        
            return playerData;
        }
    }
}