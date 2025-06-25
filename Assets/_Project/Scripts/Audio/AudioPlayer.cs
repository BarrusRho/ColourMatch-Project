namespace ColourMatch
{
    public static class AudioPlayer
    {
        private static AudioService AudioService => ServiceLocator.Get<AudioService>();

        public static void Click() => AudioService.PlayAudioClip(AudioTag.Click);
        public static void Confirm() => AudioService.PlayAudioClip(AudioTag.Confirm);
        public static void NewGameStart() => AudioService.PlayAudioClip(AudioTag.NewGameStart);
        public static void PlayerImpact() => AudioService.PlayAudioClip(AudioTag.PlayerImpact);
        public static void ChangeColour() => AudioService.PlayAudioClip(AudioTag.ChangeColour);
        public static void ObstacleSpawn() => AudioService.PlayAudioClip(AudioTag.NewObstacleSpawn);
        public static void ColourMatch() => AudioService.PlayAudioClip(AudioTag.ColourMatch);
    }
}
