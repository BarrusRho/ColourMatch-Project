namespace ColourMatch
{
    public class GameHUDController : ControllerBase<GameHUDView>
    {
        protected override void OnInit()
        {
            View.OnLeftButtonClicked += OnLeftButtonSelected;
            View.OnRightButtonClicked += OnRightButtonSelected;
        }

        private void OnLeftButtonSelected()
        {
            Logger.BasicLog(typeof(GameHUDController), "Left button clicked — event handled.", LogChannel.UI);
            EventBus.Fire(new LeftButtonClickedEvent());
        }

        private void OnRightButtonSelected()
        {
            Logger.BasicLog(typeof(GameHUDController), "Right button clicked — event handled.", LogChannel.UI);
            EventBus.Fire(new RightButtonClickedEvent());
        }
    }
}
