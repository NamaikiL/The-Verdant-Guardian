using System;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        #region Variables

        [Header("Player Follow Parameters")] 
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float collisionOffset = .2f;
        
        [Header("Camera")]
        [SerializeField] private float smoothSpeed = 1f;
        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private float pitchMin = -35f;
        [SerializeField] private float pitchMax = 60f;
        [SerializeField] private float lookAheadDistance = 2f;
        [SerializeField] private float lookAheadSmoothTime = .5f;
        
        // Camera Rotation.
        private float _yaw;      // Vertical Rotation.
        private float _pitch;    // Horizontal Rotation.
        
        // Look Ahead;
        private Vector3 _velocity;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * 
         * </summary>
         */
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        
        /**
         * <summary>
         * 
         * </summary>
         */
        void LateUpdate()
        {
            if (target.parent.GetComponent<PlayerController>().Direction.normalized.magnitude >= .1f)
            {
                _yaw = target.eulerAngles.y;
            }
            else
            {
                HandleCameraRotation();
            }

            Transform targetTransform = target;
            Vector3 desiredPosition = targetTransform.position + offset + targetTransform.forward * lookAheadDistance;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, lookAheadSmoothTime);

            HandleCollisions(smoothedPosition);
            
            transform.LookAt(targetTransform.position + targetTransform.forward * lookAheadDistance);
        }

        #endregion

        #region Camera Behavior

        /**
         * <summary>
         * Handle the camera rotation with the mouse pos.
         * </summary>
         */
        private void HandleCameraRotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            _yaw += mouseX;
            _pitch -= mouseY;
            _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

            transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
            offset = Quaternion.Euler(_pitch, _yaw, 0f) * new Vector3(5.2f, 2f, -3f);
        }


        private void HandleCollisions(Vector3 smoothedPosition)
        {
            if (Physics.Linecast(target.position, smoothedPosition, out RaycastHit hit))
            {
                smoothedPosition = hit.point + hit.normal * collisionOffset;
            }
            
            transform.position = smoothedPosition;
        }

        #endregion
    }
}
