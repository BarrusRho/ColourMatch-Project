namespace ColourMatch
{
    public class GameHUDUIController : UIControllerBase<GameHUDUIView>, IUIController
    {
        protected override void OnInit()
        {
            View.OnLeftButtonClicked += OnLeftButtonSelected;
            View.OnRightButtonClicked += OnRightButtonSelected;
        }

        private void OnLeftButtonSelected()
        {
            Logger.BasicLog(typeof(GameHUDUIController), "Left button clicked — event handled.", LogChannel.UI);
            EventBus.Fire(new LeftButtonClickedEvent());
        }

        private void OnRightButtonSelected()
        {
            Logger.BasicLog(typeof(GameHUDUIController), "Right button clicked — event handled.", LogChannel.UI);
            EventBus.Fire(new RightButtonClickedEvent());
        }
    }
}
