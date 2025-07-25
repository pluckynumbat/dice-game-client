namespace Model
{
    /// <summary>
    /// This manager holds the current state of the game and performs state transitions
    /// </summary>
    public class StateManager
    {
        public enum GameState
        {
            Auth = 0,
            MainMenu,
            LevelInProgress,
            LevelEnd,
        }
        
        public GameState CurrentState { get; private set; }
        
    }
}
