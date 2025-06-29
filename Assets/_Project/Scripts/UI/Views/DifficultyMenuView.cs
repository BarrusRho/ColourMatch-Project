namespace ColourMatch
{
    public class DifficultyMenuView : ViewBase
    {
        private int selectedDifficultyIndex = 0;
        
        public void OnDifficultySelectedButtonPressed()
        {
            var selectedDifficulty = (DifficultyLevel)selectedDifficultyIndex;
            EventBus.Fire(new DifficultySelectedEvent(selectedDifficulty));
        }

        private void SetSelectedDifficulty(int difficultyIndex)
        {
            selectedDifficultyIndex = difficultyIndex;
        }
        
        protected override void OnShow()
        {
            Logger.BasicLog(this, "Difficulty menu shown.", LogChannel.UI);
        }

        protected override void OnHide()
        {
            Logger.BasicLog(this, "Difficulty menu hidden.", LogChannel.UI);
        }

        public void OnEasyButtonPressed()
        {
            SetSelectedDifficulty(0);
        }

        public void OnMediumButtonPressed()
        {
            SetSelectedDifficulty(1);
        }

        public void OnHardButtonPressed()
        {
            SetSelectedDifficulty(2);
        }
    }
}