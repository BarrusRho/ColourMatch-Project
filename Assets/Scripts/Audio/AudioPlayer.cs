namespace ColourMatch
{
    public static class AudioPlayer
    {
        private static AudioManager _audioManager;

        public static void SetAudioManager(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public static void Click()
        {
            _audioManager.PlayAudioClip(AudioTag.Click);
        }

        public static void Confirm()
        {
            _audioManager.PlayAudioClip(AudioTag.Confirm);
        }
    }
}
