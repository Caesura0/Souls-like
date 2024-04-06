


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//not sure i like that we are using scene management name space here...
using UnityEngine.SceneManagement;


namespace JS
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
        public PlayerManager player;
        PlayerControls playerControls;
        [SerializeField] Vector2 movement;


        //These should be getters, don't love this
        public float verticalInput = 0f;
        public float horizontalInput = 0f;
        public float moveAmount = 0f;



        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;


        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool springInput = false;



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
            SceneManager.activeSceneChanged += OnSceneChange;
            Instance.enabled = false;

        }


        void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
            {
                Debug.Log("Enabled Input");
                Instance.enabled = true;
            }
            else
            {
                Instance.enabled = false;
                Debug.Log("Disabled Input");
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                //whenever we move the joystick, use lamba function to assign value to movement
                playerControls.Player.Movement.performed += i => movement = i.ReadValue<Vector2>();
                playerControls.Player.CameraControls.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.Player.Dodge.performed += i => dodgeInput = true;

                //holding input activates it till released
                playerControls.Player.Sprint.performed += i => springInput = true;
                playerControls.Player.Sprint.canceled += i => springInput = false;
            }
            playerControls.Enable();
            if (player == null)
            {
                Debug.LogError("Player is null?!");
            }

        }
        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if(focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();


        }

        private void HandleAllInputs()
        {
            HandleMovementInput();
            HandleCameraMovement();
            HandleDodgeInput();
            HandleSprinting();

        }
        //Movements
        private void HandleMovementInput()
        {
            verticalInput = movement.y;
            horizontalInput = movement.x;


            // Always makes sure we are returning a value between 0 and 1;
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
            //means we have 2 types of movement, walk and jog
            if(moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1;
            }
            else
            {
                moveAmount = 0;
            }

            Debug.Log("MoveAmount = " + moveAmount);
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0f, moveAmount, player.playerNetworkManager.isSprinting.Value);

            //if we are not locked on, only idle, 
            //i definitely do not prefer this, why does the input manager know about the player?
     

            //if we are locked on, pass both
        }
        void HandleCameraMovement()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;

        }
        //Actions
        void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
            //TODO: Return if menu or UI window is open
        }

        void HandleSprinting()
        {
            if(springInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        public void SetLocalPlayer(PlayerManager playerManager)
        {
            Debug.LogWarning(playerManager.name + " set to InputManager");
            player = playerManager;
        }

    }
}
