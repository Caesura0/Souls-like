using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JS
{
    public class PlayerManager : CharaterManager
    {
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerLocoMotionManager playerLocomotionManager;
        public PlayerNetworkManager playerNetworkManager;
        public PlayerStatManager playerStatManager;


        protected override void Awake()
        {
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocoMotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatManager = GetComponent<PlayerStatManager>();
        }
        protected override void Update()
        {
            base.Update();

            //only run movement logic if you are owner of this player
            if (!IsOwner)
            {
                return;
            }
            playerLocomotionManager.HandleAllMovement();
            playerStatManager.RegenStamina();
        }
        protected override void LateUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            base.LateUpdate();
            PlayerCamera.Instance.HandleAllCameraActions();

        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                Debug.LogWarning("NetworkSpawnOwner");
                //TODO:these should be setters or properties, i don't love public variables
                PlayerCamera.Instance.SetLocalPlayer(this);
                PlayerInputManager.Instance.SetLocalPlayer(this);
                WorldSaveGameManager.Instance.player = this;
                //subscribing to an invent with another class through a singleton?.... hmm.... this should AT THE LEAST call another method that calls the instance...
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Instance.hudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatManager.ResetStaminaReganTimer;
                playerNetworkManager.maxStamina.Value = playerStatManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                PlayerUIManager.Instance.hudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);


            }
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterSaveData.yPosition = transform.position.y;
            currentCharacterSaveData.xPosition = transform.position.x;
            currentCharacterSaveData.zPosition = transform.position.z;

        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterSaveData)
        {
            playerNetworkManager.characterName.Value = currentCharacterSaveData.characterName;
            Vector3 position = new Vector3(currentCharacterSaveData.xPosition, currentCharacterSaveData.yPosition, currentCharacterSaveData.zPosition);

            transform.position = position;
        }
    }
}
