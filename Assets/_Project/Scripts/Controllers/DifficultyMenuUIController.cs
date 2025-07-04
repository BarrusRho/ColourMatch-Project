namespace ColourMatch
{
    public class DifficultyMenuUIController : UIControllerBase<DifficultyMenuUIView>, IUIController
    {
        protected override void OnInit()
        {
            View.OnDifficultyButtonClicked += OnDifficultySelected;
        }

        private void OnDifficultySelected(DifficultyLevel difficultyLevel)
        {
            Logger.BasicLog(typeof(DifficultyMenuUIController), $"Difficulty {difficultyLevel} chosen — firing event", LogChannel.UI);
            EventBus.Fire(new DifficultySelectedEvent(difficultyLevel));
        }
    }
}
