namespace ColourMatch
{
    public class MainMenuController : ControllerBase<MainMenuView>
    {
        protected override void OnInit()
        {
            View.OnStartButtonClicked += OnOnStartGameButtonClicked;
        }

        private void OnOnStartGameButtonClicked()
        {
            Logger.BasicLog(typeof(MainMenuController), "Start button clicked (controller) — firing event.", LogChannel.UI);
            EventBus.Fire(new GameStartEvent());
        }
    }
}
