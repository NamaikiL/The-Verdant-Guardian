using System.Collections;
using UnityEngine;

namespace _Scripts.Managers
{
    public class AnimationManager : MonoBehaviour
    {
        #region Variables

        [Header("Dragon Attack")] 
        [SerializeField] private GameObject shockWavePrefab;

        // Animators.
        private Animator _playerAnimator;
        private Animator _dragonAnimator;
        
        // Singleton.
        private static AnimationManager _instance;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Unity calls Awake when an enabled script instance is being loaded.
         * </summary>
         */
        void Awake()
        {
            // Singleton.
            if(_instance) Destroy(this);
            _instance = this;
        }

        #endregion

        #region Properties
        
        // Animator.
        public Animator PlayerAnimator
        {
            get => _playerAnimator;
            set => _playerAnimator = value;
        }

        public Animator DragonAnimator
        {
            get => _dragonAnimator;
            set => _dragonAnimator = value;
        }
        
        // Singleton.
        public static AnimationManager Instance => _instance;

        #endregion

        #region Initialize Methods

        /**
         * <summary>
         * Get the player Animator.
         * </summary>
         */
        public void InitializePlayerAnimator(Animator playerAnimator)
        {
            _playerAnimator = playerAnimator;
        }
        
        
        /**
         * <summary>
         * Get the dragon Animator.
         * </summary>
         */
        public void InitializeDragonAnimator(Animator dragonAnimator)
        {
            _dragonAnimator = dragonAnimator;
        }

        #endregion

        #region Player Animations

        /**
         * <summary>
         * Update the locomotion animation.
         * </summary>
         * <param name="locomotion">The movement value.</param>
         * <param name="isSprinting">The sprinting value.</param>
         */
        public void UpdateLocomotionAnimation(float locomotion, bool isSprinting)
        {
            _playerAnimator.SetFloat($"Locomotion", locomotion);
            _playerAnimator.SetBool($"IsRunning", isSprinting);
        }

        
        /**
         * <summary>
         * Update the jumping animation.
         * </summary>
         * <param name="verticalVelocity">The vertical velocity value.</param>
         * <param name="isGrounded">The grounding value.</param>
         */
        public void UpdateJumpingAnimation(float verticalVelocity, bool isGrounded)
        {
            if (!isGrounded)
            {
                _playerAnimator.SetFloat($"Velocity", verticalVelocity);
                _playerAnimator.SetBool($"IsGrounded", false);
            }
            else _playerAnimator.SetBool($"IsGrounded", true);
        }

        
        /**
         * <summary>
         * Update the crouching animation.
         * </summary>
         * <param name="isCrouching">The crouching value.</param>
         */
        public void UpdateCrouchingAnimation(bool isCrouching)
        {
            _playerAnimator.SetBool($"IsCrouching", isCrouching);
        }

        
        /**
         * <summary>
         * Update the dodging animation.
         * </summary>
         * <param name="isDodging">The dodging value.</param>
         * <param name="dodgingDuration">The duration value.</param>
         */
        public void UpdateDodgingAnimation(bool isDodging, float dodgingDuration)
        {
            if(isDodging) _playerAnimator.SetBool($"IsDodging", true);
            Invoke("UpdateEndDodgingAnimation", dodgingDuration);
        }


        private void UpdateEndDodgingAnimation()
        {
            _playerAnimator.SetBool($"IsDodging", false);
        }

        #endregion

        #region Dragon Animation

        /**
         * <summary>
         * Update the dragon locomotion value on animator.
         * </summary>
         */
        public void UpdateDragonLocomotion(float dragonSpeed)
        {
            _dragonAnimator.SetFloat($"Locomotion", dragonSpeed);
        }
        
        
        /**
         * <summary>
         * Update the dragon fire breath animation.
         * </summary>
         */
        public void UpdateDragonFireBreathAnimation(bool isBreathing)
        {
            _dragonAnimator.SetBool($"IsBreathing", isBreathing);
        }


        /**
         * <summary>
         * Update the dragon stomp animation.
         * </summary>
         */
        public void UpdateDragonStompAnimation(bool isStomping)
        {
            _dragonAnimator.SetBool($"IsStomping", isStomping);
        }


        /**
         * <summary>
         * Shock Wave.
         * </summary>
         */
        public IEnumerator ShockWave(float time, Vector3 dragonPos)
        {
            Vector3 originalScale = new Vector3(0.1f, 3f, 0.1f);
            Vector3 targetScale = new Vector3(30f, 1f, 30f);
            float currentTime = 0f;
            
            GameObject shockWave = Instantiate(shockWavePrefab, transform.position, Quaternion.identity);
            shockWave.transform.position = dragonPos;
            shockWave.transform.localScale = originalScale;

            while (currentTime <= time)
            {
                shockWave.transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            }

            shockWave.transform.localScale = targetScale;
            Destroy(shockWave);
            yield return null;
        }

        #endregion
    }
}
