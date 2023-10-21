using UnityEngine;

namespace ColourMatch
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClipsSO audioClipsSO;

        private void PlaySoundEffect(AudioClip audioClip, Vector2 position)
        {
            AudioSource.PlayClipAtPoint(audioClip, position);
        }

        public void PlayNewGameStartAudio()
        {
            PlaySoundEffect(audioClipsSO.newGameStartAudioClip, Vector2.zero);
        }
        
        public void PlayClickAudio()
        {
            PlaySoundEffect(audioClipsSO.clickAudioCLip, Vector2.zero);
        }
        
        public void PlayConfirmButtonPressAudio()
        {
            PlaySoundEffect(audioClipsSO.confirmButtonAudioClip, Vector2.zero);
        }
        
        public void PlayNewObstacleSpawnAudio()
        {
            PlaySoundEffect(audioClipsSO.newObstacleSpawnAudioClip, Vector2.zero);
        }
        
        public void PlayChangeColourButtonAudio()
        {
            PlaySoundEffect(audioClipsSO.changeColourAudioClip, Vector2.zero);
        }

        public void PlayColourMatchAudio()
        {
            PlaySoundEffect(audioClipsSO.colourMatchAudioClip, Vector2.zero);
        }

        public void PlayPlayerImpactAudio()
        {
            PlaySoundEffect(audioClipsSO.playerImpactAudioClip, Vector2.zero);
        }
    }
}
