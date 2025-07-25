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
            LevelInProgress,
            LevelEnd,
        }
        
        public GameState CurrentState { get; private set; }

        public void ChangeGameState(GameState newState)
        {
            if (CurrentState == newState)
            {
                return;
            }

            CurrentState = newState;
        }
    }
}
