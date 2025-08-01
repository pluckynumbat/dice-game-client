using System;
using System.Threading.Tasks;
using Model;

namespace Network
{
    /// <summary>
    ///  The network request to attempt to enter a dice game level
    /// </summary>
    public class EnterLevelRequest
    {
        private const string Endpoint = "/gameplay/entry";

        public async Task<EnterLevelResponse> Send(string playerID, int level, RequestParams extraParams)
        {
            EnterLevelResponse enterLevelResponse =
                await GameRoot.Instance.NetRequestManager.SendPostRequest<EnterLevelResponse, EnterLevelRequestBody>(
                    Endpoint,
                    new EnterLevelRequestBody() { playerID = playerID, level = level },
                    extraParams);

            return enterLevelResponse;
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