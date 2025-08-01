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
        
        // The core function that sends the actual request to the server and reads the response
        private async Task<TRes> SendRequest<TRes>(RequestType requestType, string uri, string requestBody, RequestParams extraParams) where TRes : struct
        {
            TRes response = default(TRes);
            
            UnityWebRequest sentRequest = new UnityWebRequest();
            UnityWebRequestAsyncOperation webRequestOp = new UnityWebRequestAsyncOperation();
            int attemptCount = extraParams.Retries + 1;
            
            // 1. Send the request to the server (with possible retries)
            while (attemptCount > 0)
            {
                UnityWebRequest reqToSend = requestType == RequestType.Get ? UnityWebRequest.Get(uri)
                    : UnityWebRequest.Post(uri, requestBody, "application/json");
                
                reqToSend.timeout = extraParams.Timeout;
                reqToSend.SetRequestHeader("Session-Id", GameRoot.Instance.AuthManager.SessionID);
                
                webRequestOp = reqToSend.SendWebRequest();
                await webRequestOp;
                
                // check if successful / has a protocol failure, if so, move one
                if (webRequestOp.webRequest.result != UnityWebRequest.Result.ConnectionError)
                {
                    sentRequest = reqToSend;
                    break;
                }
                
                Debug.LogWarning("current request failed, retrying");
                await Task.Delay(RetryDelayMilliseconds);
                --attemptCount;
                
                // save the latest request we sent
                sentRequest = reqToSend;
            }
            
            // 2. Response extraction / error state logic
            bool markFailure = false;
            switch (webRequestOp.webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log($"success, response: {sentRequest.downloadHandler.text}");
                    response = JsonUtility.FromJson<TRes>(sentRequest.downloadHandler.text);
                    break;
                
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError($"connection failure, reason: {sentRequest.error}, the server is not online, please try again later");
                    markFailure = true;
                    response = default(TRes);
                    break;
                
                case UnityWebRequest.Result.ProtocolError:
                    // 401 is an unauthorized error which tells us that the session
                    // needs to be refreshed, so we will attempt a reload
                    if (sentRequest.responseCode == 401)
                    {
                        Debug.LogError($"http failure, reason: {sentRequest.error}, reloading might fix the issue");
                        GameRoot.Instance.ErrorManager.EnterErrorState(ErrorType.Unauthorized);
                        break;
                    }
                    markFailure = true;
                    response = default(TRes);
                    break;
            
                default:
                    Debug.LogError($"other failure, reason: {sentRequest.error}");
                    markFailure = true;
                    response = default(TRes);
                    break;
            }
            
            // this will prompt the error state with a 'Quit' button, should only be used in fatal error cases 
            if (markFailure && extraParams.QuitOnFail)
            {
                GameRoot.Instance.ErrorManager.EnterErrorState(ErrorType.CriticalError);
            }
            return response;
        }
    }
    
    // extra parameters sent by other managers to control specific aspects (usually tied to failure)
    public struct RequestParams
    {
        public int Timeout;
        public int Retries;
        public bool QuitOnFail;
    }
}
