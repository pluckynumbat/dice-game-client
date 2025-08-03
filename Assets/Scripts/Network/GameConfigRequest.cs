using System;
using System.Threading.Tasks;
using Model;

namespace Network
{
    /// <summary>
    /// The network request to get the game config
    /// </summary>
    public class GameConfigRequest
    {
        private const string Port = Constants.ConfigServerPort;
        private const string Endpoint = "/config/game-config";

        public async Task<GameConfig> Send(RequestParams extraParams)
        {
            GameConfig gameConfig = 
                await GameRoot.Instance.NetRequestManager.SendGetRequest<GameConfig>(
                    Port,Endpoint, extraParams);

            return gameConfig;
        }
    }

    [Serializable]
    public struct GameConfig
    {
        public LevelConfig[] levels;
        public int defaultLevel;
        public int maxEnergy;
        public int energyRegenSeconds;
        public int defaultLevelScore;
    }

    [Serializable]
    public struct LevelConfig
    {
        public int level;
        public int energyCost;
        public int totalRolls;
        public int target;
        public int energyRewards;
    }
}