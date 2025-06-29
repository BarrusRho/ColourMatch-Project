namespace ColourMatch
{
    public readonly struct GameBeginEvent
    {
        public DifficultyLevel DifficultyLevel { get; }

        public GameBeginEvent(DifficultyLevel difficultyLevel)
        {
            DifficultyLevel = difficultyLevel;
        }
    }
}
