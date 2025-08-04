using System;
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
        public const int RetryDelayMilliseconds = 1000;
        
        // Public helper method to send a POST request
        public async Task<TRes> SendPostRequest<TRes,TReq>(string port, string endpoint, TReq requestBody, RequestParams extraParams) where TRes : struct where TReq : struct
        {
            string postData = JsonUtility.ToJson(requestBody);
            TRes response = await SendRequest<TRes>(RequestType.Post, port, endpoint, postData, extraParams);
            return response;
        }
        
        // Public helper method to send a GET request
        public async Task<TRes> SendGetRequest<TRes>(string port, string endpoint, RequestParams extraParams) where TRes : struct
        {
            TRes response = await SendRequest<TRes>(RequestType.Get, port, endpoint, null, extraParams);
            return response;
        }

        // The core function that sends the actual request to the server and reads the response
        private async Task<TRes> SendRequest<TRes>(RequestType requestType, string port, string endpoint, string requestBody, RequestParams extraParams) where TRes : struct
        {
            TRes response = default(TRes);
            string uri = $"{Constants.ServerProtocol}://{Constants.ServerHost}:{port}{endpoint}";
            
            UnityWebRequest sentRequest = new UnityWebRequest();
            UnityWebRequestAsyncOperation webRequestOp = new UnityWebRequestAsyncOperation();
            int attemptCount = extraParams.Retries + 1;

            try
            {
                // 1. Send the request to the server (with possible retries)
                while (attemptCount > 0)
                {
                    UnityWebRequest reqToSend = requestType == RequestType.Get
                        ? UnityWebRequest.Get(uri)
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
                        Debug.LogError(
                            $"connection failure, reason: {sentRequest.error}, the server is not online, please try again later");
                        markFailure = true;
                        response = default(TRes);
                        break;

                    case UnityWebRequest.Result.ProtocolError:
                        // 401 is an unauthorized error which tells us that the session
                        // needs to be refreshed, so we will attempt a reload
                        if (sentRequest.responseCode == 401)
                        {
                            Debug.LogError($"unauthorized failure (http 401), reason: {sentRequest.error}, reloading might fix the issue");
                            GameRoot.Instance.ErrorManager.EnterErrorState(ErrorType.Unauthorized);
                            break;
                        }

                        // 404 is the not found error, but it can be an acceptable response
                        // *Only If* the caller specifies that in the extra params
                        if (sentRequest.responseCode == 404 && extraParams.IsNotFoundOk)
                        {
                            Debug.LogWarning("request returned a not found error (http 404); the params say it is ok, so not marking as failure");
                            response = default(TRes);
                            break;
                        }

                        Debug.LogError($"http protocol error, response code: {sentRequest.responseCode} reason: {sentRequest.error}");
                        markFailure = true;
                        response = default(TRes);
                        break;

                    default:
                        Debug.LogError($"other failure, reason: {sentRequest.error}");
                        markFailure = true;
                        response = default(TRes);
                        break;
                }

                // this will prompt the error state if the extra params have specified it
                if (markFailure && extraParams.ErrorOnFail != ErrorType.None)
                {
                    GameRoot.Instance.ErrorManager.EnterErrorState(extraParams.ErrorOnFail);
                }
            }
            catch (Exception exception)
            {
               Debug.LogError($"net request manager exception: {exception.Message}");
               GameRoot.Instance.ErrorManager.EnterErrorState(extraParams.ErrorOnFail);
            }


            return response;
        }
    }
    
    // extra parameters sent by other managers / callers to control specific aspects (usually tied to failure)
    public struct RequestParams
    {
        public int Timeout;
        public int Retries;
        public ErrorType DefaultErrorOnFail;
        public Dictionary<HttpStatusCode, ErrorType> CustomHttpStatusBasedErrors;
    }
}
