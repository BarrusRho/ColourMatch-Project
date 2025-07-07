using System.Collections.Generic;

namespace ColourMatch
{
    public class GameplaySystemService : MonoBehaviourServiceUser
    {
        private readonly List<IGameplaySystem> systems = new();
        private readonly List<IGameplayLoop> updateLoops = new();
        private readonly List<IFixedGameplayLoop> fixedLoops = new();

        public void Initialise() { }
        
        public void Dispose() { }

        public void RegisterSystem(IGameplaySystem system)
        {
            systems.Add(system);
            system.Initialise();

            if (system is IGameplayLoop updateLoop)
                updateLoops.Add(updateLoop);

            if (system is IFixedGameplayLoop fixedLoop)
                fixedLoops.Add(fixedLoop);
        }

        private void Update()
        {
            foreach (var loop in updateLoops)
            {
                loop.Tick();
            }
        }

        private void FixedUpdate()
        {
            foreach (var loop in fixedLoops)
            {
                loop.FixedTick();
            }
        }

        public void ResetAll()
        {
            foreach (var system in systems)
            {
                system.Reset();
            }
        }

        public void ShutdownAll()
        {
            foreach (var system in systems)
            {
                system.Shutdown();
            }
        }
    }
}