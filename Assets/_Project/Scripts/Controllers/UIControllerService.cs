using System.Collections.Generic;

namespace ColourMatch
{
    public class UIControllerService
    {
        private readonly Dictionary<ViewType, IController> _controllers = new();
        
        private readonly MainMenuController _mainMenuController;
        private readonly DifficultyMenuController _difficultyMenuController;
        private readonly GameHUDController _gameHUDController;

        public UIControllerService(UIViewsRegistry uiViewsRegistry)
        {
            _controllers[ViewType.MainMenu] = ControllerFactory.Create<MainMenuController>(uiViewsRegistry.mainMenuView);
            _controllers[ViewType.DifficultyMenu] = ControllerFactory.Create<DifficultyMenuController>(uiViewsRegistry.difficultyMenuView);
            _controllers[ViewType.GameHUD] = ControllerFactory.Create<GameHUDController>(uiViewsRegistry.gameHUDView);
        }

        public void Initialise()
        {
            
        }

        public void Show(ViewType viewType)
        {
            _controllers[viewType]?.Show();
        }

        public void Hide(ViewType viewType)
        {
            _controllers[viewType]?.Hide();
        }

        public void HideAll()
        {
            foreach (var controller in _controllers.Values)
            {
                controller.Hide();
            }
        }
        
        /*public void ShowMainMenu()
        {
            _mainMenuController.Show();
            _difficultyMenuController.Hide();
            _gameHUDController.Hide();
        }

        public void ShowDifficultyMenu()
        {
            _mainMenuController.Hide();
            _difficultyMenuController.Show();
            _gameHUDController.Hide();
        }
        
        public void ShowGameHUD()
        {
            _mainMenuController.Hide();
            _difficultyMenuController.Hide();
            _gameHUDController.Show();
        }

        public void HideAll()
        {
            _mainMenuController.Hide();
            _difficultyMenuController.Hide();
            _gameHUDController.Hide();
        }*/
    }
}
