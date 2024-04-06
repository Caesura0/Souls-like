
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace JS
{
    public class PlayerLocoMotionManager : CharaterLocomotionManager
    {
        PlayerCamera playerCamera;
        PlayerManager player;

        //MOVEMENT
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float sprintingSpeed = 8;
        [SerializeField] int sprintingStaminaCost = 2;
        [SerializeField] float rotationSpeed = 15;


        public float verticalMovement;
        public float horizontalInput;
        public float moveAmount;

        //Dodge
        Vector3 rollDirection;
        [SerializeField] float dodgeStaminaCost = 25;


        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();
            if (player.IsOwner)
            {
                player.charactorNetworkManager.verticalMovement.Value = verticalMovement; 
                player.charactorNetworkManager.horizontalMovement.Value = horizontalInput; 
                player.charactorNetworkManager.moveAmount.Value = moveAmount; 
            }
            else
            {
                verticalMovement = player.charactorNetworkManager.verticalMovement.Value;
                horizontalInput = player.charactorNetworkManager.horizontalMovement.Value ;
                moveAmount = player.charactorNetworkManager.moveAmount.Value;

                //player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            //air movement
            HandleRotation();
            //falling
        }

        void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;

            horizontalInput = PlayerInputManager.Instance.horizontalInput;

            moveAmount = PlayerInputManager.Instance.moveAmount;
            //CLAMP later
        }

        public void HandleGroundedMovement()
        {
            if (!player.canMove)
            {
                return;
            }
            GetMovementValues();

            moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalInput;
            moveDirection.Normalize();
            //no vertical movement currently
            moveDirection.y = 0;

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.Instance.moveAmount > 0.5f)
                {
                    //Move at jogging speed
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
                {
                    //walking speed
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }


        }

        private void HandleRotation()
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.Instance.cameraObject.transform.right * horizontalInput;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;


            if(targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
                
            }
            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void AttemptToPerformDodge()
        {
            if(player.isPerformingAction)
            {
                return;
            }
            if(player.playerNetworkManager.currentStamina.Value <= 0)
            {
                return;
            }
            if(moveAmount > 0)//Roll
            {
                rollDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
                rollDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalInput;

                rollDirection.y = 0;
                rollDirection.Normalize();
                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;
                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
            }
            else//Backstep
            {
                player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }
            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;

        }

        public void HandleSprinting()
        {
            if(player.isPerformingAction) 
            {
                player.playerNetworkManager.isSprinting.Value = false;
                //sprinting false
                return;
            }

            if(player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            //if stamina then sprinting false
            if (moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }

    }
}
