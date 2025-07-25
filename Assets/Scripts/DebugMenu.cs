using Network;
using Presentation;
using UnityEngine;

/// <summary>
/// Will add all sorts of functionality here to help develop the game
/// </summary>
public class DebugMenu : MonoBehaviour
{
    private bool isOpen;

    private Vector2 scrollPosition = Vector2.zero;

    private void Awake()
    {
        enabled = false;
    }

    public void ToggleState()
    {
        isOpen = !isOpen;
        enabled = isOpen;
    }
    
    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        DisplayDebugMenu();
        GUILayout.EndScrollView();
    }
    
    private void DisplayDebugMenu()
    {
        GUILayout.Label("-----Debug Menu-----", GUILayout.MaxWidth(Screen.width), GUILayout.MinHeight(20));
        GUILayout.Label($"Current Game State: {GameRoot.Instance.StateManager.CurrentState}");
        
        if (GUILayout.Button("Send a Game Config Request to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            _ = GameRoot.Instance.ConfigManager.RequestConfig();
        }
        
        if (GUILayout.Button("Send a New Player Request to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            _ = GameRoot.Instance.PlayerManager.RequestNewPlayerCreation("test1234");
        }
        
        if (GUILayout.Button("Send a Player Data to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            _ = GameRoot.Instance.PlayerManager.RequestPlayerData("test1234");
        }
        
        if (GUILayout.Button("Switch to the Loading Screen", GUILayout.MaxWidth(Screen.width)))
        {
            FindFirstObjectByType<ScreenCoordinator>().ChangeToScreen(ScreenCoordinator.ScreenType.Loading);
        }
        
        if (GUILayout.Button("Switch to the Main Screen", GUILayout.MaxWidth(Screen.width)))
        {
            FindFirstObjectByType<ScreenCoordinator>().ChangeToScreen(ScreenCoordinator.ScreenType.Main);
        }
        
        if (GUILayout.Button("Switch to the Stats Screen", GUILayout.MaxWidth(Screen.width)))
        {
            FindFirstObjectByType<ScreenCoordinator>().ChangeToScreen(ScreenCoordinator.ScreenType.Stats);
        }
        
        if (GUILayout.Button("Switch to the Gameplay Screen", GUILayout.MaxWidth(Screen.width)))
        {
            FindFirstObjectByType<ScreenCoordinator>().ChangeToScreen(ScreenCoordinator.ScreenType.Gameplay);
        }
        
        if (GUILayout.Button("Switch to the Results Screen", GUILayout.MaxWidth(Screen.width)))
        {
            FindFirstObjectByType<ScreenCoordinator>().ChangeToScreen(ScreenCoordinator.ScreenType.Result);
        }

        if (GUILayout.Button("Close the debug menu",  GUILayout.MaxWidth(Screen.width)))
        {
            ToggleState();
        }
    }
}
