using UnityEngine;

/// <summary>
/// Constants shared across the client
/// </summary>
public class Constants : MonoBehaviour
{
    public const string ServerProtocol = "http";
    public const string ServerHost = "localhost";
    
    public const string AuthServerPort = "40001";
    public const string DataServerPort = "40002";
    public const string ConfigServerPort = "40003";
    public const string ProfileServerPort = "40004";
    public const string StatsServerPort = "40005";
    public const string GameplayServerPort = "40006";
}
