using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace JS
{
    public class CharaterAnimatorManager : MonoBehaviour
    {
        CharaterManager charater;

        int verticalHash;
        int horizontalHash;

        protected virtual void Awake()
        {
            charater = GetComponent<CharaterManager>();
            verticalHash = Animator.StringToHash("Vertical");
            horizontalHash = Animator.StringToHash("Horizontal");
        }


        public void UpdateAnimatorMovementParameters(float horizontalValues, float verticalValues, bool isSprinting)
        {
            

            float horizontal = 0;
            float vertical = verticalValues;
            if (isSprinting)
            {
                Debug.LogWarning("Is Sprinting = " + isSprinting);
                vertical = 2f;
                horizontal = 2f;
            }
            else
            {
 
            }

            charater.animator.SetFloat(horizontalHash, horizontal, 0.1f, Time.deltaTime);
            charater.animator.SetFloat(verticalHash, vertical, 0.1f, Time.deltaTime);

        }
        //this should be an enum -_- STRINGS ugh
        public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformAction, bool applyrootmotion = true, bool canRotate = false, bool canMove = false)
        {
            charater.applyRootMotion = applyrootmotion;
            charater.animator.CrossFade(targetAnimation, 0.2f);
            charater.isPerformingAction = isPerformAction;
            charater.canRotate = canRotate;
            charater.canMove = canMove;

            //Tell server we played animation, play for all clients(aside from the one calling it)
            charater.charactorNetworkManager.NotifyServerOfActionAnimationServerRPC(NetworkManager.Singleton.LocalClientId, targetAnimation, applyrootmotion);
        }

    }

}