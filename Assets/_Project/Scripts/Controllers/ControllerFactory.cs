namespace ColourMatch
{
    public static class ControllerFactory
    {
        public static TController Create<TController>(IView view) where TController : IController, new()
        {
            var controller = new TController();
            controller.Init(view);
            return controller;
        }
    }
}
