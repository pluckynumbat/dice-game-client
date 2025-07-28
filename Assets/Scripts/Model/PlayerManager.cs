using System.Threading.Tasks;
using Network;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// Holds data and functionality related to the player
    /// </summary>
    public class PlayerManager
    {
        // TODO: should these constants come from the config instead?
        public const int MaximumEnergy = 50;
        public const int EnergyRegenerationSeconds = 5;
        
        public PlayerData PlayerData;
        public PlayerStats Stats;
        
        public async Task RequestNewPlayerCreation(string playerID)
        {
            NewPlayerRequest request = new NewPlayerRequest();
            PlayerData responseData = await request.Send(GameRoot.Instance.AuthManager.SessionID, playerID);

            if (string.IsNullOrEmpty(responseData.playerID))
            {
                Debug.LogError("could not create the new player :(");
                return;
            }

            PlayerData = responseData;
        }
        
        public async Task RequestPlayerData(string playerID)
        {
            PlayerDataRequest request = new PlayerDataRequest();
            PlayerData responseData = await request.Send(GameRoot.Instance.AuthManager.SessionID, playerID);

            if (string.IsNullOrEmpty(responseData.playerID))
            {
                Debug.LogError("could not get the player data :(");
                return;
            }
            
            PlayerData = responseData;
        }
        
        public async Task RequestPlayerStats(string playerID)
        {
            PlayerStatsRequest request = new PlayerStatsRequest();
            PlayerStats responseData = await request.Send(GameRoot.Instance.AuthManager.SessionID, playerID);

            if (responseData.levelStats == null)
            {
                Debug.Log("no player stats yet :(");
                return;
            }
            
            Stats = responseData;
        }

        public void UpdatePlayerData(PlayerData newData)
        {
            PlayerData = newData;
        }
        
        public void UpdateStats(PlayerStats newStats)
        {
            Stats = newStats;
        }
    }
}
