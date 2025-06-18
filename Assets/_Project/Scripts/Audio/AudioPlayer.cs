using UnityEngine;

namespace ColourMatch
{
    public static class AudioPlayer
    {
        private static AudioManager _audioManager;

        private static AudioManager AudioManager
        {
            get
            {
                if (!_audioManager)
                {
                    _audioManager = Object.FindObjectOfType<AudioManager>();
                }

                return _audioManager;
            }
        }

        public static void Click()
        {
            AudioManager.PlayAudioClip(AudioTag.Click);
        }

        public static void Confirm()
        {
            AudioManager.PlayAudioClip(AudioTag.Confirm);
        }
    }
}
