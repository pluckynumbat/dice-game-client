using System;
using System.Threading.Tasks;
using Model;

namespace Network
{
    /// <summary>
    ///  The network request to get the result of the dice game level, and updated player data (and stats) along with it
    /// </summary>
    public class LevelResultRequest
    {
        private const string Endpoint = "/gameplay/result";
    
        public async Task<LevelResultResponse> Send(string playerID, int level, int[] rolls, RequestParams extraParams)
        {
            LevelResultResponse levelResultResponse =  
                await GameRoot.Instance.NetRequestManager.SendPostRequest<LevelResultResponse, LevelResultRequestBody>
                    (Endpoint, new LevelResultRequestBody() {playerID = playerID, level = level, rolls = rolls}, extraParams);
            
            return levelResultResponse;
        }
    }
    
    [Serializable]
    public struct LevelResultRequestBody
    {
        public string playerID;
        public int level;
        public int[] rolls;
    }

    [Serializable]
    public struct LevelResultResponse
    {
        public LevelResult levelResult;
        public PlayerData playerData;
        public PlayerStats statsData;
    }
    
    [Serializable]
    public struct LevelResult
    {
        public bool won;
        public int energyReward;
        public bool unlockedNewLevel;
    }

}