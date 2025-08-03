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
        public PlayerData PlayerData;
        public PlayerStats Stats;
        
        public async Task RequestNewPlayerCreation(string playerID, RequestParams extraParams)
        {
            NewPlayerRequest request = new NewPlayerRequest();
            PlayerData responseData = await request.Send(playerID, extraParams);

            if (string.IsNullOrEmpty(responseData.playerID))
            {
                Debug.LogError("could not create the new player :(");
                return;
            }

            PlayerData = responseData;
        }
        
        public async Task RequestPlayerData(string playerID, RequestParams extraParams)
        {
            PlayerDataRequest request = new PlayerDataRequest();
            PlayerData responseData = await request.Send(playerID, extraParams);

            if (string.IsNullOrEmpty(responseData.playerID))
            {
                Debug.LogError("could not get the player data :(");
                return;
            }
            
            PlayerData = responseData;
        }
        
        public async Task RequestPlayerStats(string playerID, RequestParams extraParams)
        {
            PlayerStatsRequest request = new PlayerStatsRequest();
            PlayerStatsResponse response = await request.Send(playerID, extraParams);

            if (string.IsNullOrEmpty(response.playerID))
            {
                Debug.Log("failed to get player stats :(");
                return;
            }
            
            Stats = response.playerStats;
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
