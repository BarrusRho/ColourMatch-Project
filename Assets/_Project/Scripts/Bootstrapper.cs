using System.Threading.Tasks;
using UnityEngine;

namespace ColourMatch
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField]
        private GameCamera gameCamera;

        [SerializeField] private StateManager stateManager;
        
        [SerializeField] private AudioClipsSO audioClips;
        [SerializeField] private AudioSource[] audioSources;
        
        private async void Awake()
        { 
            await InitialiseComponentsAsync();
        }

        private async Task InitialiseComponentsAsync()
        {
            await gameCamera.InitialiseAsync();
            await AudioManager.Instance.InitialiseAsync(audioClips, audioSources);
            await stateManager.InitialiseAsync();
        }
    }
}
