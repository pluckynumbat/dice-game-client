#define DEBUG_MENU_ENABLED // comment this when not in debug mode anymore

using UnityEngine;

/// <summary>
/// Singleton that provides access to the different client services
/// </summary>
public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance { get; private set; }
    
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
        
#if DEBUG_MENU_ENABLED
        debugMenu = gameObject.AddComponent<DebugMenu>();
#endif
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
