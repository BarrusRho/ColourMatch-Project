using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColourMatch
{
    public class ControllerService
    {
        private readonly Dictionary<ViewType, IController> _controllers = new();

        public ControllerService(ViewRegistry viewRegistry)
        {
            foreach (var viewEntry in viewRegistry.views)
            {
                if (viewEntry.view == null)
                {
                    Logger.Error(
                        typeof(ControllerService),
                        $"[ControllerService] View reference is null for ViewType: {viewEntry.viewType}",
                        LogChannel.ControllerService
                    );
                    continue;
                }

                if (_controllers.ContainsKey(viewEntry.viewType))
                {
                    Logger.Warning(
                        typeof(ControllerService),
                        $"[ControllerService] Duplicate ViewType detected: {viewEntry.viewType}",
                        LogChannel.UI
                    );
                    continue;
                }

                var controller = ControllerFactory.Create(viewEntry.viewType, viewEntry.view);
                _controllers[viewEntry.viewType] = controller;

                Logger.BasicLog(
                    typeof(ControllerService),
                    $"[ControllerService] Registered {viewEntry.viewType} with {controller.GetType().Name}",
                    LogChannel.ControllerService
                );
            }
        }

        public void Initialise()
        {
            EventBus.Subscribe<ViewTransitionEvent>(OnViewTransition);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<ViewTransitionEvent>(OnViewTransition);
        }

        private void OnViewTransition(ViewTransitionEvent viewTransitionEvent)
        {
            Logger.BasicLog(typeof(ControllerService), $"ViewTransitionEvent received: showing {viewTransitionEvent.ViewToShow}", LogChannel.ControllerService);
            HideAll();
            Show(viewTransitionEvent.ViewToShow);
        }

        public void Show(ViewType viewType)
        {
            if (_controllers.TryGetValue(viewType, out var controller))
                controller.Show();
            else
                Logger.Warning(typeof(ControllerService), $"No controller for {viewType}", LogChannel.ControllerService);
        }

        public void Hide(ViewType viewType)
        {
            if (_controllers.TryGetValue(viewType, out var controller))
                controller.Hide();
            else
                Logger.Warning(typeof(ControllerService), $"No controller for {viewType}", LogChannel.ControllerService);
        }

        public void HideAll()
        {
            foreach (var controller in _controllers.Values)
            {
                controller.Hide();
            }
        }
        
        public async Task ShowAsync(ViewType viewType)
        {
            if (_controllers.TryGetValue(viewType, out var controller))
                await controller.ShowAsync();
            else
                Logger.Warning(typeof(ControllerService), $"No controller for {viewType}", LogChannel.ControllerService);
        }

        public async Task HideAsync(ViewType viewType)
        {
            if (_controllers.TryGetValue(viewType, out var controller))
                await controller.HideAsync();
            else
                Logger.Warning(typeof(ControllerService), $"No controller for {viewType}", LogChannel.ControllerService);
        }

        public async Task HideAllAsync()
        {
            foreach (var controller in _controllers.Values)
            {
                await controller.HideAsync();
            }
        }
        
        public IController GetController(ViewType viewType)
        {
            if (_controllers.TryGetValue(viewType, out var controller))
            {
                return controller;
            }

            Logger.Warning(typeof(ControllerService), $"GetController: No controller registered for {viewType}", LogChannel.ControllerService);
            return null;
        }
        
        public T GetController<T>(ViewType viewType) where T : class, IController
        {
            if (_controllers.TryGetValue(viewType, out var controller))
            {
                if (controller is T typedController)
                    return typedController;

                Logger.Error(typeof(ControllerService),
                    $"GetController<{typeof(T).Name}>: Controller for {viewType} is not of type {typeof(T).Name}. It is {controller.GetType().Name}.",
                    LogChannel.ControllerService);
                return null;
            }

            Logger.Warning(typeof(ControllerService), $"GetController<{typeof(T).Name}>: No controller registered for {viewType}", LogChannel.ControllerService);
            return null;
        }
    }
}
