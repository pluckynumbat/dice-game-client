using Presentation;

namespace Model
{
    /// <summary>
    /// This manager holds the current state of the game and performs state transitions
    /// </summary>
    public class StateManager
    {
        public enum GameState
        {
            None = 0,
            Auth,
            MainMenu,
            ViewingStats,
            LevelInProgress,
            LevelEnd,
        }
        
        public GameState CurrentState { get; private set; }

        private ScreenCoordinator screenCoordinator;

        public StateManager(ScreenCoordinator coordinator)
        {
            screenCoordinator = coordinator;
        }

        public void InitializeScreenCoordinator(GameRoot root)
        {
            screenCoordinator.InitializeScreens(root);
        }

        public void ChangeGameState(GameState newState)
        {
            if (CurrentState == newState)
            {
                return;
            }

            GameState previousState = CurrentState;
            CurrentState = newState;

            switch (newState)
            {
                case GameState.Auth:
                    screenCoordinator.ChangeToScreen(ScreenCoordinator.ScreenType.Loading);
                    break;
                
                case GameState.MainMenu:
                    screenCoordinator.ChangeToScreen(ScreenCoordinator.ScreenType.Main);
                    break;
                
                case GameState.ViewingStats:
                    screenCoordinator.ChangeToScreen(ScreenCoordinator.ScreenType.Stats);
                    break;
                
                case GameState.LevelInProgress:
                    screenCoordinator.ChangeToScreen(ScreenCoordinator.ScreenType.Gameplay);
                    break;
                
                case GameState.LevelEnd:
                    screenCoordinator.ChangeToScreen(ScreenCoordinator.ScreenType.Result);
                    break;
            }
        }
    }
}
