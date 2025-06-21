namespace ColourMatch
{
    public static class AudioPlayer
    {
        private static AudioManager AudioManager => AudioManager.Instance;

        public static void Click() => AudioManager.PlayAudioClip(AudioTag.Click);
        public static void Confirm() => AudioManager.PlayAudioClip(AudioTag.Confirm);
        public static void NewGameStart() => AudioManager.PlayAudioClip(AudioTag.NewGameStart);
        public static void PlayerImpact() => AudioManager.PlayAudioClip(AudioTag.PlayerImpact);
        public static void ChangeColour() => AudioManager.PlayAudioClip(AudioTag.ChangeColour);
        public static void ObstacleSpawn() => AudioManager.PlayAudioClip(AudioTag.NewObstacleSpawn);
        public static void ColourMatch() => AudioManager.PlayAudioClip(AudioTag.ColourMatch);
    }
}
