using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ColourMatch
{
    public class AudioManager
    {
        private AudioClipsSO _audioClipsSO;
        private AudioSource[] _audioSources;
        
        public AudioManager() { }

        public void Initialise(AudioClipsSO audioClips, AudioSource[] audioSources)
        {
            _audioClipsSO = audioClips;
            _audioSources = audioSources;
        }

        public void PlayAudioClip(AudioTag audioTag)
        {
            if (!_audioClipsSO.HasAudioClip(audioTag))
            {
                Debug.LogWarning($"{audioTag} does not exist");
                return;
            }
            
            var audioClip = _audioClipsSO.GetAudioClip(audioTag);
            var audioSource = _audioSources.FirstOrDefault(x => !x.isPlaying);
            if (audioSource == null)
            {
                Debug.LogWarning($"No free audio sources available");
                return;
            }
            
            audioSource.PlayOneShot(audioClip);
        }

    }
}
