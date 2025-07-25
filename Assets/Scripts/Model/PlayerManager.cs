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
        
        public async Task RequestNewPlayerCreation(string playerID)
        {
            NewPlayerRequest request = new NewPlayerRequest();
            PlayerData responseData = await request.Send(playerID);

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
            PlayerData responseData = await request.Send(playerID);

            if (string.IsNullOrEmpty(responseData.playerID))
            {
                Debug.LogError("could not get the player data :(");
                return;
            }
            
            PlayerData = responseData;
        }
        
        // TODO: add method that updates the energy
    }
}
