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
            Loading = 0,
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
    
        // TODO: add error screen!
    
        private ScreenType currentScreen;

        public void ChangeToScreen(ScreenType newScreenType)
        {
            if (currentScreen == newScreenType)
            {
                return;
            }

            DisableAllScreens();

            switch (newScreenType)
            {
                case ScreenType.Loading:
                    LoadingScreen?.gameObject.SetActive(true);
                    break;
            
                case ScreenType.Main:
                    MainScreen?.UpdateDisplay();
                    MainScreen?.gameObject.SetActive(true);
                    break;
            
                case ScreenType.Stats:
                    StatsScreen?.gameObject.SetActive(true);
                    break;
            
                case ScreenType.Gameplay:
                    GameplayScreen?.gameObject.SetActive(true);
                    break;
            
                case ScreenType.Result:
                    ResultScreen?.gameObject.SetActive(true);
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
        }
    }
}
