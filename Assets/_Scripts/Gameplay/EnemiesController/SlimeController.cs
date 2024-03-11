using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class SlimeController : EnemyController
    {
        #region Variables

        //Component.
        private AnimationManager _animationManager;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start ()
        {
            _animationManager = AnimationManager.Instance;
        }

        /**
         * <summary>
         * Update is called every frame, if the MonoBehaviour is enabled.
         * </summary>
         */
        void Update ()
        {
            //UpdateAnimation();
        }

        #endregion

        #region Animations Methods

        /**
         * <summary>
         * Update the animations.
         * </summary>
         */
        private void UpdateAnimation ()
        {
            _animationManager.UpdateSlimeLocomotion(0.5f);
        }

        #endregion
    }
}