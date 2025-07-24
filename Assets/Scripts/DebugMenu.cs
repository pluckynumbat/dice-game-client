using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private bool isOpen;

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
        DisplayDebugMenu();
    }
    
    private void DisplayDebugMenu()
    {
        if (GUILayout.Button("Close the debug menu",  GUILayout.MaxWidth(Screen.width)))
        {
            ToggleState();
        }
    }
}
