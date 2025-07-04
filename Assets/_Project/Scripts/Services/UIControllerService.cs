using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColourMatch
{
    public class UIControllerService
    {
        private readonly Dictionary<UIViewType, IUIController> controllers = new();

        public UIControllerService(UIViewRegistry uiViewRegistry)
        {
            foreach (var viewEntry in uiViewRegistry.views)
            {
                if (viewEntry.uiView is not IUIView uiView)
                {
                    Logger.Error(
                        typeof(UIControllerService),
                        $"Skipped registration — {viewEntry.uiView?.GetType().Name ?? "null"} is not an IUIView for {viewEntry.uiViewType}",
                        LogChannel.UIControllerService
                    );
                    continue;
                }

                if (controllers.ContainsKey(viewEntry.uiViewType))
                {
                    Logger.Warning(
                        typeof(UIControllerService),
                        $"Duplicate ViewType detected: {viewEntry.uiViewType}",
                        LogChannel.UIControllerService
                    );
                    continue;
                }

                var controller = UIControllerFactory.Create(viewEntry.uiViewType, viewEntry.uiView);
                controllers[viewEntry.uiViewType] = controller;

                Logger.BasicLog(
                    typeof(UIControllerService),
                    $"Registered {viewEntry.uiViewType} with {controller.GetType().Name}",
                    LogChannel.UIControllerService
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
            
            Clear();
            Logger.BasicLog(typeof(UIControllerService), "Disposed UIControllerService and its controllers.", LogChannel.UIControllerService);
        }

        private void OnViewTransition(ViewTransitionEvent viewTransitionEvent)
        {
            Logger.BasicLog(typeof(UIControllerService), $"ViewTransitionEvent received: showing {viewTransitionEvent.UIViewToShow}", LogChannel.UIControllerService);
            HideAll();
            Show(viewTransitionEvent.UIViewToShow);
        }

        public void Show(UIViewType uiViewType)
        {
            if (controllers.TryGetValue(uiViewType, out var controller))
                controller.Show();
            else
                Logger.Warning(typeof(UIControllerService), $"No controller for {uiViewType}", LogChannel.UIControllerService);
        }

        public void Hide(UIViewType uiViewType)
        {
            if (controllers.TryGetValue(uiViewType, out var controller))
                controller.Hide();
            else
                Logger.Warning(typeof(UIControllerService), $"No controller for {uiViewType}", LogChannel.UIControllerService);
        }

        public void HideAll()
        {
            foreach (var controller in controllers.Values)
            {
                controller.Hide();
            }
        }
        
        public async Task ShowAsync(UIViewType uiViewType)
        {
            if (controllers.TryGetValue(uiViewType, out var controller))
                await controller.ShowAsync();
            else
                Logger.Warning(typeof(UIControllerService), $"No controller for {uiViewType}", LogChannel.UIControllerService);
        }

        public async Task HideAsync(UIViewType uiViewType)
        {
            if (controllers.TryGetValue(uiViewType, out var controller))
                await controller.HideAsync();
            else
                Logger.Warning(typeof(UIControllerService), $"No controller for {uiViewType}", LogChannel.UIControllerService);
        }

        public async Task HideAllAsync()
        {
            foreach (var controller in controllers.Values)
            {
                await controller.HideAsync();
            }
        }
        
        public IUIController GetController(UIViewType uiViewType)
        {
            if (controllers.TryGetValue(uiViewType, out var controller))
            {
                return controller;
            }

            Logger.Warning(typeof(UIControllerService), $"No controller found for {uiViewType}", LogChannel.UIControllerService);
            return null;
        }
        
        public T GetController<T>(UIViewType uiViewType) where T : class, IUIController
        {
            if (controllers.TryGetValue(uiViewType, out var controller))
            {
                if (controller is T typedController)
                    return typedController;

                Logger.Error(typeof(UIControllerService),
                    $"Type mismatch: Controller for {uiViewType} is {controller.GetType().Name}, expected {typeof(T).Name}",
                    LogChannel.UIControllerService);
                return null;
            }

            Logger.Warning(typeof(UIControllerService), $"GetController<{typeof(T).Name}>: No controller registered for {uiViewType}", LogChannel.UIControllerService);
            return null;
        }
        
        private IUIController TryGet(UIViewType uiViewType)
        {
            if (controllers.TryGetValue(uiViewType, out var controller))
                return controller;

            Logger.Warning(typeof(UIControllerService), $"No controller for {uiViewType}", LogChannel.UIControllerService);
            return null;
        }
        
        public bool Unregister(UIViewType uiViewType)
        {
            if (controllers.Remove(uiViewType))
            {
                Logger.BasicLog(typeof(UIControllerService), $"Unregistered UI controller: {uiViewType}", LogChannel.UIControllerService);
                return true;
            }

            Logger.Warning(typeof(UIControllerService), $"Tried to unregister non-existent UI controller: {uiViewType}", LogChannel.UIControllerService);
            return false;
        }
        
        public void Clear()
        {
            if (controllers.Count == 0) return;
            
            foreach (var controller in controllers.Values)
            {
                if (controller is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            
            controllers.Clear();
            Logger.BasicLog(typeof(UIControllerService), "Cleared all UI controllers.", LogChannel.UIControllerService);
        }
    }
}
