namespace ColourMatch
{
    public readonly struct DifficultySelectedEvent
    {
        public DifficultyLevel DifficultyLevel { get; }

        public DifficultySelectedEvent(DifficultyLevel difficultyLevel)
        {
            DifficultyLevel = difficultyLevel;
        }
    }
}
