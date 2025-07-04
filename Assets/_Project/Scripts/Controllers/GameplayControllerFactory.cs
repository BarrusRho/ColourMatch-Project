using System;

namespace ColourMatch
{
    public static class GameplayControllerFactory
    {
        public static IGameplayController Create(GameplayControllerType type, IGameplayView view)
        {
            return type switch
            {
                GameplayControllerType.PlayerController => Create<PlayerController>(view),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        private static T Create<T>(IGameplayView view) where T : IGameplayController, new()
        {
            var controller = new T();
            controller.Init((GameplayViewBase)view);
            return controller;
        }
    }

}
