using System;
using UnityEngine;

namespace ColourMatch
{
    public class GameInitialiser : MonoBehaviour
    {
        [SerializeField] private AudioManager _audioManager;

        private void Awake()
        {
                InitialiseComponents();
        }

        private void InitialiseComponents()
        {
            AudioPlayer.SetAudioManager(_audioManager);
        }
    }
}
