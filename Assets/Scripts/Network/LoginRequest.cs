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