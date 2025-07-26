using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    ///  The network request to authenticate a new or existing user
    /// </summary>
    public class LoginRequest
    {
        private const string ServerHost = "http://localhost";
        private const string ServerPort = "8080";
        private const string Endpoint = "/auth/login";
    
        public async Task<AuthLoginData> Send(string authString, bool isNewUser, int timeout = 10)
        {
            AuthLoginData authLoginData;
        
            string uri = $"{ServerHost}:{ServerPort}{Endpoint}";
            
            LoginRequestBody requestBody = new LoginRequestBody() {isNewUser = isNewUser};
            string postData = JsonUtility.ToJson(requestBody);
            
            UnityWebRequest postRequest = UnityWebRequest.Post(uri,postData, "application/json");
            
            postRequest.timeout = timeout;
            postRequest.SetRequestHeader("Authorization", authString); // add the authorization header

            await postRequest.SendWebRequest();
        
            switch (postRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log($"success! your player id is: {postRequest.downloadHandler.text}");
                    authLoginData.loginResponse = JsonUtility.FromJson<LoginResponse>(postRequest.downloadHandler.text);
                    authLoginData.sessionID = postRequest.GetResponseHeader("Session-Id"); // get the session id from the header
                    Debug.Log($"session id: {authLoginData.sessionID}");
                    break;

                default:
                    Debug.LogError($"failure, reason: {postRequest.error}");
                    authLoginData = default(AuthLoginData);
                    break;
            }
        
            return authLoginData;
        }
    }
    
    [Serializable]
    public struct LoginRequestBody
    {
        public bool isNewUser;
    }
    
    [Serializable]
    public struct AuthLoginData
    {
        public LoginResponse loginResponse;
        public string sessionID;
    }
    
    [Serializable]
    public struct LoginResponse
    {
        public string playerID;
    }
}