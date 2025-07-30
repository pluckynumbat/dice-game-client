#define DEBUG_MENU_ENABLED // comment this when not in debug mode anymore

using Model;
using Presentation;
using UnityEngine;

/// <summary>
/// Singleton that provides access to the different client services
/// </summary>
public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance { get; private set; }

    public ConfigManager ConfigManager { get; private set; }
    public StateManager StateManager { get; private set; }
    public PlayerManager PlayerManager  { get; private set; }
    public AuthManager AuthManager  { get; private set; }
    public GameplayManager GameplayManager  { get; private set; }
    public ErrorManager ErrorManager  { get; private set; }
    
    private DebugMenu debugMenu;

    private void Awake()
    {
        // set this up as a singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);

        // find the screen coordinator script in the bootstrap scene
        ScreenCoordinator screenCoordinator = FindFirstObjectByType<ScreenCoordinator>();
        
        // create the different managers
        ConfigManager = new ConfigManager();
        StateManager = new StateManager(screenCoordinator);
        PlayerManager = new PlayerManager();
        AuthManager = new AuthManager();
        GameplayManager = new GameplayManager();
        ErrorManager = new ErrorManager();
        
#if DEBUG_MENU_ENABLED
        debugMenu = gameObject.AddComponent<DebugMenu>();
#endif
        
        // initialize the screen coordinator
        StateManager.InitializeScreenCoordinator(this);
        
        // start the game with the auth flow!
        _ = AuthManager.PerformAuthFlow(screenCoordinator.LoadingScreen);
    }

    private void Update()
    {
#if DEBUG_MENU_ENABLED
        if (Input.GetKeyDown(KeyCode.D))
        {
            debugMenu?.ToggleState();
        }
#endif
    }

    private void OnDestroy()
    {
        _ = AuthManager.RequestLogout();
    }
}
