using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    ///  The network request to log the user out by deleting the current session
    /// </summary>
    public class LogoutRequest
    {
        private const string ServerHost = "http://localhost";
        private const string ServerPort = "8080";
        private const string Endpoint = "/auth/logout";
    
        public async Task<bool> Send(string sessionID, int timeout = 5)
        {
            bool success;
            
            string uri = $"{ServerHost}:{ServerPort}{Endpoint}";
            
            UnityWebRequest deleteRequest = UnityWebRequest.Delete(uri);
            deleteRequest.timeout = timeout;
            deleteRequest.SetRequestHeader("Session-Id", sessionID);

            await deleteRequest.SendWebRequest();
        
            switch (deleteRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log("success! you were logged out");
                    success = true;
                    break;

                default:
                    Debug.LogError($"failure, reason: {deleteRequest.error}");
                    success = false;
                    break;
            }
            return success;
        }
    }
}