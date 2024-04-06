
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JS
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance;
        public PlayerManager player;
        public Camera cameraObject;


        [SerializeField] Transform cameraPivotTransform;

     
        [Header("Camera Settings")]


        float cameraSmoothSpeed = 1f; //Bigger number means longer time to reach target
        [SerializeField] float leftRightRotationSpeed = 220f;
        [SerializeField] float upDownRotationSpeed = 220f;
        [SerializeField] float minPivot = -30;
        [SerializeField] float maxPivot = 60;
        [SerializeField] float cameraCollisionRadius = .2f;
        [SerializeField] LayerMask cameraCollisionLayers;


        [Header("Camera Values")]
        Vector3 cameraVelocity;
        Vector3 cameraObjectPosition;//USED for Camera collisions
        [SerializeField] float leftRightLookingAngle;
        [SerializeField] float upDownLookingAngle;
        float cameraZPosition; //used for camera collisions
        float targetCameraZPosition; //used for camera collisions


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
            cameraZPosition = cameraObject.transform.localPosition.z ;
        }


        public void HandleAllCameraActions()
        {

            if (player != null)
            {
                HandleFollowTarget();
                HandleRotation();
                HandleCollision();
            }
        }


        void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;

        }

        void HandleRotation()
        {
            //TODO:If locked on, rotate toward target
            leftRightLookingAngle += (PlayerInputManager.Instance.cameraHorizontalInput * leftRightRotationSpeed) * Time.deltaTime;
            upDownLookingAngle -= (PlayerInputManager.Instance.cameraVerticalInput * upDownRotationSpeed) * Time.deltaTime;
            upDownLookingAngle = Mathf.Clamp(upDownLookingAngle, minPivot, maxPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            //handle left and right first
            cameraRotation.y = leftRightLookingAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            //reset camera rotation now that we have applied it
            cameraRotation = Vector3.zero;
            //handle up and down seperate
            cameraRotation.x = upDownLookingAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }


        void HandleCollision()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), 0))
            {
                float distanceFromObjectCollided = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromObjectCollided - cameraCollisionRadius);
            }

            if(Mathf.Abs(targetCameraZPosition)< cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, cameraCollisionRadius);
        }


        public void SetLocalPlayer(PlayerManager playerManager)
        {
            Debug.LogWarning(playerManager.name + " set to camera");
            player = playerManager;
        }
    }




}