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
        
        // create the different managers
        ConfigManager = new ConfigManager();
        StateManager = new StateManager(this, FindFirstObjectByType<ScreenCoordinator>());
        
#if DEBUG_MENU_ENABLED
        debugMenu = gameObject.AddComponent<DebugMenu>();
#endif
        
        // start the game in the auth state!
        StateManager.ChangeGameState(StateManager.GameState.Auth);
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
}
