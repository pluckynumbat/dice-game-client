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
        
        public async Task RequestNewPlayerCreation(string playerID)
        {
            NewPlayerRequest request = new NewPlayerRequest();
            PlayerData responseData = await request.Send(playerID, new RequestParams() {Timeout = 10, Retries = 1, ErrorOnFail = ErrorType.CriticalError});

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
            PlayerData responseData = await request.Send(playerID, new RequestParams() {Timeout = 10, Retries = 1, ErrorOnFail = ErrorType.CouldNotConnect});

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
            PlayerStatsResponse response = await request.Send(playerID, new RequestParams() {Timeout = 5, Retries = 0});

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
