using UnityEngine;

namespace _Scripts.Gameplay
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [Header("Player Properties")]
        [SerializeField] private Transform target;

        [Header("Camera Properties")]
        [SerializeField] private float smoothSpeed = 0.2f;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 2f, -7f);

        [Header("Camera Rotation Properties")]
        [SerializeField] private float mouseSensitivity = 150f;
        [SerializeField] private float pitchMin = -30f;
        [SerializeField] private float pitchMax = 60f;

        [Header("Camera Movement Following")]
        [SerializeField] private float movementSensitivity = 2f; // Adjust this value to control how much the camera's yaw changes with player movement

        [Header("Collision Properties")]
        [SerializeField] private float collisionOffset = 1.5f;

        private float _pitch;
        private float _yaw;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _yaw = transform.eulerAngles.y;
            _pitch = transform.eulerAngles.x;
        }

        void LateUpdate()
        {
            if (!target) return;

            HandleCameraRotation();

            Vector3 desiredCameraPosition = target.position + Quaternion.Euler(_pitch, _yaw, 0) * cameraOffset;
            Vector3 collisionAdjustedPosition = CameraCollisionAdjustment(target.position, desiredCameraPosition);
            transform.position = Vector3.Lerp(transform.position, collisionAdjustedPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(target);
        }

        private void HandleCameraRotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            float playerMovementDirection = Input.GetAxis("Horizontal"); // Assuming "Horizontal" maps to player's left/right movement

            _yaw += mouseX + (playerMovementDirection * movementSensitivity); // Modify yaw based on player movement as well as mouse input
            _pitch -= mouseY;
            _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);
        }

        private Vector3 CameraCollisionAdjustment(Vector3 fromPosition, Vector3 toPosition)
        {
            if (Physics.Linecast(fromPosition, toPosition, out RaycastHit hit))
            {
                return hit.point + hit.normal * collisionOffset;
            }
            return toPosition;
        }
    }
}