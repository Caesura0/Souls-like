using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace JS
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Instance;

        [Header("NEWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        public PlayerUIHudManager hudManager;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            hudManager = GetComponentInChildren<PlayerUIHudManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                // We must first shut down, because we have started as a host during the title screen
                NetworkManager.Singleton.Shutdown();
                // We then restart as a client
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}

