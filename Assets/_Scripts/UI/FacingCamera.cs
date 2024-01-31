using UnityEngine;

namespace _Scripts.UI
{
    public class FacingCamera : MonoBehaviour
    {
        #region Variables

        private Camera _camera;

        #endregion

        #region Built-In Methods

        /**
     * <summary>
     * Start is called before the first frame update
     * </summary>
     */
        void Start()
        {
            _camera = Camera.main;
        }

        /**
     * <summary>
     * Update is called once per frame.
     * </summary>
     */
        void Update()
        {
            transform.LookAt(_camera.transform, Vector3.up);
        }

        #endregion
    
    }
}
