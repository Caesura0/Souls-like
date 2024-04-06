using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace JS
{
    public class CharaterManager : NetworkBehaviour
    {
        //why are these public instead of getters and setters if we are hiding in inspector?
        public CharacterController characterController;
        public Animator animator;
        [HideInInspector] public CharaterNetworkManager charactorNetworkManager;



        //Flags
        public bool isPerformingAction;
        public bool canRotate = true;
        public bool canMove = true;
        public bool applyRootMotion = false;




        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            characterController = GetComponent<CharacterController>();
            charactorNetworkManager = GetComponent<CharaterNetworkManager>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {

            if (IsOwner)
            {
                charactorNetworkManager.networkPosition.Value = transform.position;
                charactorNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, charactorNetworkManager.networkPosition.Value, ref charactorNetworkManager.networkPositionVelocity, charactorNetworkManager.networkPositionSmoothTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, charactorNetworkManager.networkRotation.Value, charactorNetworkManager.networkRotationSmoothTime);
            }
        }
        protected virtual void LateUpdate()
        {

        }

    }
}

