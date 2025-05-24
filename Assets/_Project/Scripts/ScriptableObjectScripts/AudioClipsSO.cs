using System.Linq;
using UnityEngine;

namespace ColourMatch
{
    [CreateAssetMenu(fileName = "AudioClips", menuName = "ColourMatch/AudioClips")]
    public class AudioClipsSO : ScriptableObject
    {
        public SoundEffectClip[] audioClips;

        public bool HasAudioClip(string audioTag)
        {
            return audioClips.Count(x => x.audioTag.Equals(audioTag)) != 0;
        }

        public AudioClip GetAudioClip(string audioTag)
        {
            return audioClips.FirstOrDefault(x => x.audioTag.Equals(audioTag))?.audioClip;
        }
    }
}
