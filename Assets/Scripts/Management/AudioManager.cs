using System;
using System.Linq;
using UnityEngine;

namespace ColourMatch
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClipsSO audioClipsSO;
        [SerializeField] private AudioSource[] audioSources;

        private void Awake()
        {
            AudioPlayer.SetAudioManager(this);
        }

        public void PlayAudioClip(string audioTag)
        {
            if (!audioClipsSO.HasAudioClip(audioTag))
            {
                Debug.LogWarning($"{audioTag} does not exist");
                return;
            }
            
            var audioClip = audioClipsSO.GetAudioClip(audioTag);
            var audioSource = audioSources.FirstOrDefault(x => !x.isPlaying);
            if (audioSource == null)
            {
                Debug.LogWarning($"No free audio sources available");
                return;
            }
            
            audioSource.PlayOneShot(audioClip);
        }

        public void PlayAudioClip(AudioTag audioTag)
        {
            PlayAudioClip(audioTag.ToString());
        }
    }
}
