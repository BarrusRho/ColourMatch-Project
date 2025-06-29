namespace ColourMatch
{
    public class DifficultyMenuController : ControllerBase<DifficultyMenuView>
    {
        protected override void OnInit()
        {
            View.OnDifficultyButtonClicked += OnDifficultySelected;
        }

        private void OnDifficultySelected(DifficultyLevel difficultyLevel)
        {
            Logger.BasicLog(typeof(DifficultyMenuController), $"Difficulty {difficultyLevel} chosen — firing event", LogChannel.UI);
            EventBus.Fire(new DifficultySelectedEvent(difficultyLevel));
        }
    }
}
