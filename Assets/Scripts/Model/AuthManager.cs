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
    }
}
