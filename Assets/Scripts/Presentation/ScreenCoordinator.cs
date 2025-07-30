using Presentation.Error.Screen;
using Presentation.Gameplay.Screen;
using Presentation.Loading.Screen;
using Presentation.Main.Screen;
using Presentation.Results.Screen;
using Presentation.Stats.Screens;
using UnityEngine;

namespace Presentation
{
    /// <summary>
    /// Class that helps switch through the different screens
    /// </summary>
    public class ScreenCoordinator : MonoBehaviour
    {
        public enum ScreenType
        {
            None = 0,
            Loading,
            Main,
            Stats,
            Gameplay,
            Result,
            Error
        }
    
        public LoadingScreen LoadingScreen;
        public MainScreen MainScreen;
        public StatsScreen StatsScreen;
        public GameplayScreen GameplayScreen;
        public ResultsScreen ResultScreen;
        public ErrorScreen ErrorScreen;
    
        private ScreenType currentScreen;

        public void InitializeScreens(GameRoot root)
        { 
           // inject dependencies of the screens into them
           MainScreen.Initialize(root.ConfigManager, root.PlayerManager, root.GameplayManager, root.ErrorManager, root.StateManager);
           StatsScreen.Initialize(root.PlayerManager, root.StateManager);
           GameplayScreen.Initialize(root.GameplayManager, root.StateManager);
           ResultScreen.Initialize(root.PlayerManager, root.GameplayManager, root.StateManager);
           ErrorScreen.Initialize(root.AuthManager, root.ErrorManager);
        }

        public void ChangeToScreen(ScreenType newScreenType)
        {
            if (currentScreen == newScreenType)
            {
                return;
            }

            // assumption is that a state that goes into the error should just go back to that state
            // if we recover from the error and dismiss the error screen, except for the loading state
            if (currentScreen == ScreenType.Error && newScreenType != ScreenType.Loading)
            {
                ErrorScreen?.gameObject.SetActive(false);
                currentScreen = newScreenType;
                return;
            }

            switch (newScreenType)
            {
                case ScreenType.Loading:
                    DisableAllScreens();
                    LoadingScreen?.gameObject.SetActive(true);
                    break;
            
                case ScreenType.Main:
                    if (currentScreen == ScreenType.Stats)
                    {
                        StatsScreen?.gameObject.SetActive(false);
                    }
                    else
                    {
                        DisableAllScreens();
                        MainScreen?.gameObject.SetActive(true);
                    }
                    break;
            
                case ScreenType.Stats:
                    StatsScreen?.gameObject.SetActive(true);
                    break;
            
                case ScreenType.Gameplay:
                    DisableAllScreens();
                    GameplayScreen?.gameObject.SetActive(true);
                    break;
            
                case ScreenType.Result:
                    ResultScreen?.gameObject.SetActive(true);
                    break;
                
                case ScreenType.Error:
                    ErrorScreen?.gameObject.SetActive(true);
                    break;
            }

            currentScreen = newScreenType;
        }

        private void DisableAllScreens()
        {
            LoadingScreen?.gameObject.SetActive(false);
            MainScreen?.gameObject.SetActive(false);
            StatsScreen?.gameObject.SetActive(false);
            GameplayScreen?.gameObject.SetActive(false);
            ResultScreen?.gameObject.SetActive(false);
            ErrorScreen?.gameObject.SetActive(false);
        }
    }
}
