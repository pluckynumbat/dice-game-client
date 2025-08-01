using System.Threading.Tasks;
using Network;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// Deals with functionality related to the game config like fetching it etc.
    /// </summary>
    public class ConfigManager
    {
        public GameConfig GameConfig { get; private set; }
        
        public async Task RequestConfig()
        {
            GameConfigRequest request = new GameConfigRequest();
            GameConfig responseData = await request.Send(new RequestParams() {Timeout = 10, Retries = 1, ErrorOnFail = ErrorType.CouldNotConnect});
            
            if (responseData.levels == null)
            {
                Debug.LogError("could not get the game config :(");
                return;
            }

            GameConfig = responseData;
        }
    }
}
