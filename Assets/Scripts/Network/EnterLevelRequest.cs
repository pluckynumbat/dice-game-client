using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    ///  The network request to attempt to enter a dice game level
    /// </summary>
    public class EnterLevelRequest
    {
        private const string ServerHost = "http://localhost";
        private const string ServerPort = "8080";
        private const string Endpoint = "/gameplay/entry";
    
        public async Task<EnterLevelResponse> Send(string sessionID, string playerID, int level, int timeout = 30)
        {
            EnterLevelResponse response;
        
            string uri = $"{ServerHost}:{ServerPort}{Endpoint}";

            EnterLevelRequestBody requestBody = new EnterLevelRequestBody() {playerID = playerID, level = level};
            string postData = JsonUtility.ToJson(requestBody);
        
            UnityWebRequest postRequest = UnityWebRequest.Post(uri,postData, "application/json");
            postRequest.timeout = timeout;
        
            postRequest.SetRequestHeader("Session-Id", sessionID);

            await postRequest.SendWebRequest();
        
            switch (postRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log($"success, enter level response: {postRequest.downloadHandler.text}");
                    response = JsonUtility.FromJson<EnterLevelResponse>(postRequest.downloadHandler.text);
                    break;

                default:
                    Debug.LogError($"failure, reason: {postRequest.error}");
                    response = default(EnterLevelResponse);
                    break;
            }
        
            return response;
        }
    }
    
    [Serializable]
    public struct EnterLevelRequestBody
    {
        public string playerID;
        public int level;
    }

    [Serializable]
    public struct EnterLevelResponse
    {
        public bool accessGranted;
        public PlayerData playerData;
    }
}