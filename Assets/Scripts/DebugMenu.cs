using UnityEngine;

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
        if (GUILayout.Button("Close the debug menu",  GUILayout.MaxWidth(Screen.width)))
        {
            ToggleState();
        }
    }
}
