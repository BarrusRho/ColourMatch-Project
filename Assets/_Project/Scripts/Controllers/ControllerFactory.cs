using System;

namespace ColourMatch
{
    public static class ControllerFactory
    {
        public static IController Create(ViewType viewType, ViewBase view)
        {
            return viewType switch
            {
                ViewType.MainMenu => Create<MainMenuController>(view),
                ViewType.DifficultyMenu => Create<DifficultyMenuController>(view),
                ViewType.GameHUD => Create<GameHUDController>(view),
                _ => throw new ArgumentOutOfRangeException(nameof(viewType), $"No controller mapped for {viewType}")
            };
        }

        private static T Create<T>(IView view) where T : IController, new()
        {
            var controller = new T();
            controller.Init(view);
            return controller;
        }
    }
}
