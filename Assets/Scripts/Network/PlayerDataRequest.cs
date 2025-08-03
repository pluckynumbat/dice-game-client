using System.Threading.Tasks;
using Model;

namespace Network
{
    /// <summary>
    /// The network request to get the data of an existing player
    /// </summary>
    public class PlayerDataRequest
    {
        private const string Port = Constants.ProfileServerPort;
        private const string Endpoint = "/profile/player-data/";

        public async Task<PlayerData> Send(string playerID, RequestParams extraParams)
        {
            PlayerData playerData =
                await GameRoot.Instance.NetRequestManager.SendGetRequest<PlayerData>(
                Port, $"{Endpoint}{playerID}", extraParams);

            return playerData;
        }
    }
}