using System.Threading.Tasks;

namespace ColourMatch
{
    public abstract class UIControllerBase<TView> : IUIController where TView : UIViewBase
    {
        protected TView View;

        public void Init(IView view)
        {
            View = (TView)view;
            OnInit();
        }

        protected abstract void OnInit();
        public virtual void Show() => View.Show();
        public virtual void Hide() => View.Hide();
        public virtual Task ShowAsync() => View.ShowAsync();
        public virtual Task HideAsync() => View.HideAsync();
    }
}