namespace ColourMatch
{
    public class MainMenuUIController : UIControllerBase<MainMenuUIView>, IUIController
    {
        protected override void OnInit()
        {
            View.OnStartGameButtonClicked += OnStartGameSelected;
        }

        private void OnStartGameSelected()
        {
            Logger.BasicLog(typeof(MainMenuUIController), "Start button clicked (controller) — firing event.", LogChannel.UI);
            EventBus.Fire(new GameStartEvent());
        }
    }
}
