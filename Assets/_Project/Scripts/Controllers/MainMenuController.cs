namespace ColourMatch
{
    public class MainMenuController : ControllerBase<MainMenuView>
    {
        protected override void OnInit()
        {
            View.OnStartGameButtonClicked += OnStartGameSelected;
        }

        private void OnStartGameSelected()
        {
            Logger.BasicLog(typeof(MainMenuController), "Start button clicked (controller) — firing event.", LogChannel.UI);
            EventBus.Fire(new GameStartEvent());
        }
    }
}
