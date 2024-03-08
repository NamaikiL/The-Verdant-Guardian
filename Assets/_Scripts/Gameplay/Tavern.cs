using System.Collections;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class Tavern : MonoBehaviour
    {
        #region Variables

        [Header("Interaction")]
        [SerializeField] private GameObject interactionUI;

        [Header("Sleep")]
        [SerializeField] private GameObject sleepAnimation;

        //Coroutine Variable.
        private float _sleepingTime = 5f;

        // Component.
        private AudioManager _audioManager;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start()
        {
            // Component.
            _audioManager = AudioManager.Instance;
        }

        /**
         * <summary>
         * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (interactionUI)
                {
                    interactionUI.SetActive(true);

                    //EVENT
                    PlayerStats.OnInteraction += PlayerSleep;
                }
            }
        }

        /**
         * <summary>
         * OnTriggerExit is called when the Collider other has stopped touching the trigger.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        void OnTriggerExit(Collider other)
        {
            if (interactionUI)
            {
                interactionUI.SetActive(false);

                //EVENT
                PlayerStats.OnInteraction -= PlayerSleep;
            }
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Effect to make the player understand he has slept.
         * </summary>
         */
        private void PlayerSleep(PlayerStats stats)
        {
            StartCoroutine(DelaySleep());
        }

        private IEnumerator DelaySleep()
        {
            _audioManager.PlayerSleepSFX.Play();
            sleepAnimation.SetActive(true);
            interactionUI.SetActive(false);

            yield return new WaitForSeconds(_sleepingTime);
            sleepAnimation.SetActive(false);
        }

        #endregion
    }
}