using System;

namespace ColourMatch
{
    public static class UIControllerFactory
    {
        public static IUIController Create(UIViewType uiViewType, IUIView uiView)
        {
            return uiViewType switch
            {
                UIViewType.MainMenu => Create<MainMenuUIController>(uiView),
                UIViewType.DifficultyMenu => Create<DifficultyMenuUIController>(uiView),
                UIViewType.GameHUD => Create<GameHUDUIController>(uiView),
                _ => throw new ArgumentOutOfRangeException(nameof(uiViewType), $"No controller mapped for {uiViewType}")
            };
        }

        private static T Create<T>(IUIView view) where T : IUIController, new()
        {
            var controller = new T();
            controller.Init(view);
            return controller;
        }
    }
}
