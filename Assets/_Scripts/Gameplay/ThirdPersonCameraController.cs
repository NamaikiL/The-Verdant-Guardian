using UnityEngine;

namespace _Scripts.Gameplay
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [Header("Player Properties")]
        [SerializeField] private Transform target;

        [Header("Camera Properties")]
        [SerializeField] private float positionSmoothTime = 0.1f;
        private Vector3 positionVelocity = Vector3.zero;

        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 2f, -7f);

        [Header("Camera Rotation Properties")]
        [SerializeField] private float mouseSensitivity = 150f;
        [SerializeField] private float pitchMin = -30f;
        [SerializeField] private float pitchMax = 60f;
        [SerializeField] private float rotationSmoothTime = 0.1f;
        private Vector2 rotationVelocity;
        private Vector2 currentRotation;

        [Header("Collision Properties")]
        [SerializeField] private float collisionOffset = 1.5f;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            currentRotation.x = transform.eulerAngles.y;
            currentRotation.y = transform.eulerAngles.x;
        }

        void FixedUpdate()
        {
            if (!target) return;

            HandleCameraRotation();

            Vector3 desiredCameraPosition = target.position + Quaternion.Euler(currentRotation.y, currentRotation.x, 0) * cameraOffset;
            Vector3 collisionAdjustedPosition = CameraCollisionAdjustment(target.position, desiredCameraPosition);
            transform.position = Vector3.SmoothDamp(transform.position, collisionAdjustedPosition, ref positionVelocity, positionSmoothTime);

            transform.LookAt(target);
        }

        private void HandleCameraRotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            currentRotation.x += mouseX;
            currentRotation.y -= mouseY;
            currentRotation.y = Mathf.Clamp(currentRotation.y, pitchMin, pitchMax);

            var rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSmoothTime);
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