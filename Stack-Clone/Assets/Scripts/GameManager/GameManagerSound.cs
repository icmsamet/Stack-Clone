using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class GameManagerSound
    {
        AudioSource audioSource;

        public GameManagerSound(AudioSource _audioSource)
        {
            audioSource = _audioSource;
        }

        public void PlayOneShot(AudioClip source)
        {
            audioSource.PlayOneShot(source);
        }
    }
}
