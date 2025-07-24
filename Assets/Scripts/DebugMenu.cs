using Network;
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
        if (GUILayout.Button("Send a Game Config Request to the server",  GUILayout.MaxWidth(Screen.width)))
        {
            GameConfigRequest request = new GameConfigRequest();
            _ = request.Send();
        }
        
        if (GUILayout.Button("Close the debug menu",  GUILayout.MaxWidth(Screen.width)))
        {
            ToggleState();
        }
    }
}
