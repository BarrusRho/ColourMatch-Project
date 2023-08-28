using UnityEngine;

namespace ColourMatch
{
    [CreateAssetMenu(fileName = "AudioClips", menuName = "DodgyBoxes/AudioClips")]
    public class AudioClipsSO : ScriptableObject
    {
        public AudioClip newGameStartAudioClip;
        public AudioClip clickAudioCLip;
        public AudioClip confirmButtonAudioClip;
        public AudioClip newObstacleSpawnAudioClip;
        public AudioClip changeColourAudioClip;
        public AudioClip colourMatchAudioClip;
        public AudioClip playerImpactAudioClip;
    }
}
