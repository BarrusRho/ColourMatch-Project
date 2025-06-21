namespace ColourMatch
{
    public abstract class ManagerBase<T> where T : ManagerBase<T>, new()
    {
        private static T _instance;

        protected ManagerBase()
        {
            if (_instance != null)
            {
                throw new System.Exception($"An instance of {typeof(T)} already exists. Singleton violation.");
            }

            _instance = (T)this;
        }

        public static T Instance => _instance ??= new T();

        public virtual void Initialise() { }
    }
}
