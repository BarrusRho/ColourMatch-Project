using UnityEngine;

namespace ColourMatch
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClipsSO audioCLipsSO;

        private void PlaySoundEffect(AudioClip audioClip, Vector2 position)
        {
            AudioSource.PlayClipAtPoint(audioClip, position);
        }

        public void PlayNewGameStartAudio()
        {
            PlaySoundEffect(audioCLipsSO.newGameStartAudioClip, Vector2.zero);
        }
        
        public void PlayClickAudio()
        {
            PlaySoundEffect(audioCLipsSO.clickAudioCLip, Vector2.zero);
        }
        
        public void PlayConfirmButtonPressAudio()
        {
            PlaySoundEffect(audioCLipsSO.confirmButtonAudioClip, Vector2.zero);
        }
        
        public void PlayNewObstacleSpawnAudio()
        {
            PlaySoundEffect(audioCLipsSO.newObstacleSpawnAudioClip, Vector2.zero);
        }
        
        public void PlayChangeColourButtonAudio()
        {
            PlaySoundEffect(audioCLipsSO.changeColourAudioClip, Vector2.zero);
        }

        public void PlayColourMatchAudio()
        {
            PlaySoundEffect(audioCLipsSO.colourMatchAudioClip, Vector2.zero);
        }

        public void PlayPlayerImpactAudio()
        {
            PlaySoundEffect(audioCLipsSO.playerImpactAudioClip, Vector2.zero);
        }
    }
}
