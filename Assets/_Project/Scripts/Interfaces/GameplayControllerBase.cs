namespace ColourMatch
{
    public abstract class GameplayControllerBase<TView> : IGameplayController where TView : GameplayViewBase
    {
        protected TView View;

        public void Init(GameplayViewBase view)
        {
            Init((TView)view);
        }
        
        public virtual void Init(TView view)
        {
            View = view;
            OnInit();
        }

        protected abstract void OnInit();
    }
}

