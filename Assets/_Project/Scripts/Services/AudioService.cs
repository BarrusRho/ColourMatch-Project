using System.Linq;
using UnityEngine;

namespace ColourMatch
{
    public class AudioService
    {
        private AudioClipsSO _audioClipsSO;
        private AudioSource[] _audioSources;
        
        public AudioService() { }

        public void Initialise(AudioClipsSO audioClips, AudioSource[] audioSources)
        {
            _audioClipsSO = audioClips;
            _audioSources = audioSources;
        }

        public void PlayAudioClip(AudioTag audioTag)
        {
            if (!_audioClipsSO.HasAudioClip(audioTag))
            {
                Logger.Error(typeof(AudioService), $"{audioTag} does not exist in AudioClipsSO", LogChannel.Audio);
                return;
            }
            
            var audioClip = _audioClipsSO.GetAudioClip(audioTag);
            var audioSource = _audioSources.FirstOrDefault(x => !x.isPlaying);
            if (audioSource == null)
            {
                Logger.Warning(typeof(AudioService), "No free audio sources available", LogChannel.Audio);
                return;
            }
            
            audioSource.PlayOneShot(audioClip);
        }

    }
}
