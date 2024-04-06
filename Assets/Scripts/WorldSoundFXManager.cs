using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        //action sound effects
        public AudioClip rollingSoundEffect;

        public static WorldSoundFXManager Instance;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject); 
        }

        
    }
}
