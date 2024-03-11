using _Scripts.Gameplay.CharactersController.Player;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        #region Variables

        [Header("Player Properties")]
        [SerializeField] private Transform target;

        [Header("Camera Properties")]
        [SerializeField] private float positionSmoothTime = 0.1f;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 2f, -7f);
        [SerializeField] private Vector3 crouchCameraOffset = new Vector3(0f, 2f, -7f);
        [SerializeField] private Vector3 dodgingCameraOffset = new Vector3(0f, 2f, -7f);
        
        [Header("Camera Rotation Properties")]
        [SerializeField] private float mouseSensitivity = 150f;
        [SerializeField] private float pitchMin = -30f;
        [SerializeField] private float pitchMax = 60f;
        [SerializeField] private float rotationSmoothTime = 0.1f;
        
        [Header("Dynamic FOV")]
        [SerializeField] private Camera cam;
        [SerializeField] private float normalFOV = 40f;
        [SerializeField] private float crouchingFOV = 20f;
        [SerializeField] private float rollingFOV = 60f;
        [SerializeField] private float runningFOV = 55f;
        [SerializeField] private float fovTransitionSpeed = 10f;

        [Header("Collision Properties")]
        [SerializeField] private float collisionOffset = 1.5f;

        [Header("Terrain Following Properties")]
        [SerializeField] private float groundDistance = 2.0f;
        [SerializeField] private LayerMask groundLayer;

        // Camera Properties.
        private Vector3 _positionVelocity = Vector3.zero;
        private Vector3 _currentOffset;
        
        // Camera Rotation Properties.
        private Vector2 _rotationVelocity;
        private Vector2 _currentRotation;
        
        // DynamicFOV.
        private float _targetFOV;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start()
        {
            // Locking the cursor.
            Cursor.lockState = CursorLockMode.Locked;

            // Getting the Current Camera Rotation.
            Vector3 cameraCurrentRotation = transform.eulerAngles;
            _currentRotation.x = cameraCurrentRotation.y;
            _currentRotation.y = cameraCurrentRotation.x;

            // Giving the base offsets.
            _currentOffset = cameraOffset;
            
            // Initializing the default field of view.
            _targetFOV = cam.fieldOfView;
        }

        
        /**
         * <summary>
         * FixedUpdate is called every fixed frame.
         * </summary>
         * <remarks>Using FixedUpdate() instead of LateUpdate() because the camera is shaking when using with physics movements.</remarks>
         */
        void FixedUpdate()
        {
            if (!target) return;    // If there's a target.

            HandleDynamicFOV();
            HandleCameraRotation();

            Vector3 targetPosition = target.position;
            Vector3 desiredCameraPosition = targetPosition + Quaternion.Euler(_currentRotation.y, _currentRotation.x, 0) * _currentOffset;
            Vector3 collisionAdjustedPosition = CameraCollisionAdjustment(targetPosition, desiredCameraPosition);
            Vector3 terrainAdjustedPosition = AdjustForTerrain(collisionAdjustedPosition);
            transform.position = Vector3.SmoothDamp(transform.position, terrainAdjustedPosition, ref _positionVelocity, positionSmoothTime);

            transform.LookAt(target);
        }

        #endregion
        
        #region Camera Modification Methods

        /**
         * <summary>
         * Handle the camera rotation by the inputs given by the player.
         * </summary>
         */
        private void HandleCameraRotation()
        {
            // Getting the inputs.
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            
            _currentRotation.x += mouseX;
            _currentRotation.y -= mouseY;
            _currentRotation.y = Mathf.Clamp(_currentRotation.y, pitchMin, pitchMax); // Clamp the pitch.

            Quaternion rotation = Quaternion.Euler(_currentRotation.y, _currentRotation.x, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSmoothTime);
        }

        
        /**
         * <summary>
         * Adjust smoothly the camera when there's a collider between the camera and the player.
         * </summary>
         * <returns>The position of the camera.</returns>
         */
        private Vector3 CameraCollisionAdjustment(Vector3 fromPosition, Vector3 toPosition)
        {
            if (Physics.Linecast(fromPosition, toPosition, out RaycastHit hit))
            {
                return hit.point + hit.normal * collisionOffset;
            }
            
            return toPosition;
        }

        
        /**
         * <summary>
         * Adapt the camera to the terrain.
         * </summary>
         * <returns>The position of the camera.</returns>
         */
        private Vector3 AdjustForTerrain(Vector3 position)
        {
            if (Physics.Raycast(position + Vector3.up * 100, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                float terrainHeight = hit.point.y + groundDistance;
                position.y = Mathf.Max(position.y, terrainHeight);
            }
            
            return position;
        }

        
        /**
         * <summary>
         * Change the FOV based on the player movements.
         * </summary>
         */
        private void HandleDynamicFOV()
        {
            if(target.parent.TryGetComponent(out PlayerController playerController)
               && target.parent.TryGetComponent(out PlayerInputs playerInputs)
               && target.parent.TryGetComponent(out PlayerStats playerStats))
            {
                if (playerInputs.Sprint 
                    && playerController.MoveDirection.normalized.magnitude > .1f 
                    && playerStats.CurrentPlayerStamina > 0f 
                    && !playerController.IsCrouching)
                {
                    _currentOffset = cameraOffset;
                    _targetFOV = runningFOV;
                }
                else if (playerController.IsCrouching)
                {
                    _currentOffset = crouchCameraOffset;
                    _targetFOV = crouchingFOV;
                }
                else if (playerController.IsDodging)
                {
                    _currentOffset = dodgingCameraOffset;
                    _targetFOV = rollingFOV;
                }
                else
                {
                    _currentOffset = cameraOffset;
                    _targetFOV = normalFOV;
                }
            }

            // Smoothly translate to the target FOV.
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, _targetFOV, Time.deltaTime * fovTransitionSpeed);
        }

        #endregion
    }
}