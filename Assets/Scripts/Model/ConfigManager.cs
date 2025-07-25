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
    }
}
