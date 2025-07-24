using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    /// The network request to get the game config
    /// </summary>
    public class GameConfigRequest
    {

    }

    [Serializable]
    public class GameConfig
    {
        public LevelConfig[] levels;
    }

    [Serializable]
    public class LevelConfig
    {
        public int level;
        public int energyCost;
        public int totalRolls;
        public int target;
        public int energyRewards;
    }
}