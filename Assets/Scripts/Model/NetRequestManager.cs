using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Model
{
    public enum RequestType
    {
        Get = 0,
        Post,
    }

    /// <summary>
    /// This will be used by other systems to send the different requests to the server
    /// </summary>
    public class NetRequestManager
    {
        private const string ServerHost = "http://localhost";
        private const string ServerPort = "8080";
        private const int RetryDelayMilliseconds = 1000;
    }
    
    // extra parameters sent by other managers to control specific aspects (usually tied to failure)
    public struct RequestParams
    {
        public int Timeout;
        public int Retries;
        public bool QuitOnFail;
    }
}
