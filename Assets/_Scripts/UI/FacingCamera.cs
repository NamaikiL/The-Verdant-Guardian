using UnityEngine;

namespace _Scripts.UI
{
    
    /**
     * <summary>
     * Making the UI in world space facing the player.
     * </summary>
     */
    public class FacingCamera : MonoBehaviour
    {
        #region Variables

        // Camera.
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
