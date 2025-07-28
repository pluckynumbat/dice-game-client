using Presentation;
using UnityEngine;

/// <summary>
/// Will add all sorts of functionality here to help develop the game
/// </summary>
public class DebugMenu : MonoBehaviour
{
    private bool isOpen;

    private Vector2 scrollPosition = Vector2.zero;

    private string username1 = "username";
    private string password1 = "password";
    
    private string username2 = "username";
    private string password2 = "password";

    private string playerID1 = "player id";

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
        
        GUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width));
        
        if (GUILayout.Button("Send a Login Request \n (as a new user) \n to the server",  GUILayout.MaxWidth(Screen.width * 0.5f),  GUILayout.MinHeight(50)))
        {
            string authHeaderPayload = GameRoot.Instance.AuthManager.CreateBasicAuthHeaderPayload(username1, password1);
            _ = GameRoot.Instance.AuthManager.RequestLogin(authHeaderPayload, true);
        }
        
        GUILayout.BeginVertical();
        username1 = GUILayout.TextArea(username1);
        password1 = (password1 == "password") ? GUILayout.TextArea(password1) : GUILayout.PasswordField(password1, '*');
        GUILayout.EndVertical();
        
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width));
        
        if (GUILayout.Button("Send a Login Request \n (as an existing user) \n to the server",  GUILayout.MaxWidth(Screen.width * 0.5f),  GUILayout.MinHeight(50)))
        {
            string authHeaderPayload = GameRoot.Instance.AuthManager.CreateBasicAuthHeaderPayload(username2, password2);
            _ = GameRoot.Instance.AuthManager.RequestLogin(authHeaderPayload, false);
        }
        
        GUILayout.BeginVertical();
        username2 = GUILayout.TextArea(username2);
        password2 = (password2 == "password") ? GUILayout.TextArea(password2) : GUILayout.PasswordField(password2, '*');
        GUILayout.EndVertical();
        
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Send a Game Config Request to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            _ = GameRoot.Instance.ConfigManager.RequestConfig();
        }
        
        if (GUILayout.Button("Send a New Player Request to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            _ = GameRoot.Instance.PlayerManager.RequestNewPlayerCreation("test1234");
        }
        
        if (GUILayout.Button("Send a Player Data Request to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            _ = GameRoot.Instance.PlayerManager.RequestPlayerData("test1234");
        }
        
        GUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width));
        if (GUILayout.Button("Send a Stats Request \n to the server",  GUILayout.MaxWidth(Screen.width * 0.5f), GUILayout.MinHeight(50)))
        {
            _ = GameRoot.Instance.PlayerManager.RequestPlayerStats(playerID1);
        }
        playerID1 = GUILayout.TextArea(playerID1);
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Send an Enter Level Request to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            _ = GameRoot.Instance.GameplayManager.RequestLevelEntry(1);
        }
        
        if (GUILayout.Button("Send a Level Result Request (roll 6) to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            _ = GameRoot.Instance.GameplayManager.RequestLevelResult(new []{1, 6});
        }
        
        if (GUILayout.Button("Send an Level Result Request (roll 1) to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            _ = GameRoot.Instance.GameplayManager.RequestLevelResult(new []{1, 1});
        }
        
        if (GUILayout.Button("Send a Logout Request to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            _ = GameRoot.Instance.AuthManager.RequestLogout();
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
        
        if (GUILayout.Button("Clear all Player Prefs",  GUILayout.MaxWidth(Screen.width)))
        {
            PlayerPrefs.DeleteAll();
        }

        if (GUILayout.Button("Close the debug menu",  GUILayout.MaxWidth(Screen.width)))
        {
            ToggleState();
        }
    }
}
