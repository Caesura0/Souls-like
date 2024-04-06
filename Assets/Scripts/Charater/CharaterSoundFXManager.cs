using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class CharaterSoundFXManager : MonoBehaviour
    {

        AudioSource audioSource;

        protected void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRollSoundFX()
        {
            
            //audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollingSoundEffect);
        }
    }
}
