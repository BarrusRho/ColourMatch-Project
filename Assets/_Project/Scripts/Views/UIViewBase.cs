using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ColourMatch
{
    public abstract class UIViewBase : MonoBehaviour, IUIView
    {
        protected bool IsDestroyed { get; private set; }
        protected bool IsTransitioning { get; private set; }

        private CancellationTokenSource transitionCts;

        private bool isViewVisible = false;
        public virtual bool IsVisible => isViewVisible;

        public virtual void Show()
        {
            if (IsVisible)
            {
                Logger.Warning(typeof(UIViewBase), $"Show() skipped — already visible: {GetType().Name}", LogChannel.UI);
                return;
            }

            Logger.BasicLog(this, "Show() called — activating GameObject.", LogChannel.UI);

            CancelTransitionIfRunning();
            gameObject.SetActive(true);
            isViewVisible = true;
            OnShow();
        }

        public virtual void Hide()
        {
            if (!IsVisible)
            {
                Logger.Warning(typeof(UIViewBase), $"Hide() skipped — already hidden: {GetType().Name}", LogChannel.UI);
                return;
            }

            Logger.BasicLog(this, "Hide() called — deactivating GameObject.", LogChannel.UI);

            CancelTransitionIfRunning();
            isViewVisible = false;
            gameObject.SetActive(false);
            OnHide();
        }

        public virtual async Task ShowAsync()
        {
            if (IsTransitioning || IsVisible)
            {
                Logger.Warning(typeof(UIViewBase), $"ShowAsync() skipped — already transitioning or visible: {GetType().Name}", LogChannel.UI);
                return;
            }

            CancelTransitionIfRunning();
            transitionCts = new CancellationTokenSource();
            IsTransitioning = true;
            
            Logger.BasicLog(this, "ShowAsync() started.", LogChannel.UI);

            gameObject.SetActive(true);
            isViewVisible = true;

            try
            {
                await OnShowAsync(transitionCts.Token);
                OnShown();
                Logger.BasicLog(this, "ShowAsync() complete.", LogChannel.UI);
            }
            catch (TaskCanceledException)
            {
                Logger.Warning(typeof(UIViewBase), $"ShowAsync() was cancelled: {GetType().Name}", LogChannel.UI);
            }

            IsTransitioning = false;
        }

        public virtual async Task HideAsync()
        {
            if (IsTransitioning || !IsVisible)
            {
                Logger.Warning(typeof(UIViewBase), $"HideAsync() skipped — already transitioning or not visible: {GetType().Name}", LogChannel.UI);
                return;
            }

            CancelTransitionIfRunning();
            transitionCts = new CancellationTokenSource();
            IsTransitioning = true;
            
            Logger.BasicLog(this, "HideAsync() started.", LogChannel.UI);

            try
            {
                await OnHideAsync(transitionCts.Token);
                isViewVisible = false;
                gameObject.SetActive(false);
                OnHidden();
                Logger.BasicLog(this, "HideAsync() complete.", LogChannel.UI);
            }
            catch (TaskCanceledException)
            {
                Logger.Warning(typeof(UIViewBase), $"HideAsync() was cancelled: {GetType().Name}", LogChannel.UI);
            }

            IsTransitioning = false;
        }

        protected virtual Task OnShowAsync(CancellationToken token) => Task.CompletedTask;
        protected virtual Task OnHideAsync(CancellationToken token) => Task.CompletedTask;

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
        protected virtual void OnShown() { }
        protected virtual void OnHidden() { }

        protected virtual void OnDestroy()
        {
            Logger.BasicLog(this, "ViewBase destroyed — cleaning up transitions.", LogChannel.UI);
            CancelTransitionIfRunning();
            IsDestroyed = true;
        }

        private void CancelTransitionIfRunning()
        {
            if (transitionCts != null)
            {
                Logger.BasicLog(this, "Cancelling in-progress transition.", LogChannel.UI);

                if (!transitionCts.IsCancellationRequested)
                {
                    transitionCts.Cancel();
                }
                
                transitionCts.Dispose();
                transitionCts = null;
            }
        }
    }
}