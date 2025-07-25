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
    }
}