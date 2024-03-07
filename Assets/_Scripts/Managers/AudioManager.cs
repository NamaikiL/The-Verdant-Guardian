using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Managers
{
    /**
     * <summary>
     * The management of the sfx and musics in game
     * </summary>
     */

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        #region Variables

        [Header("Ambient Sound")]
        [SerializeField] private AudioSource ambientSound;

        [Header("UI SFX")]
        [SerializeField] private AudioSource btnSound;

        //Singleton
        private static AudioManager _instance;

        #endregion

        #region Properties

        //User Interface SFX
        public AudioSource BtnSound => btnSound;

        // Singleton Property.
        public static AudioManager Instance => _instance;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Awake is called when an enabled script instance is being loaded.
         * </summary>
         */
        void Awake()
        {
            // Singleton.
            if (_instance) Destroy(this);
            _instance = this;
        }

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        private void Start()
        {
            if (ambientSound)
            {
                ambientSound.Play();
            }
        }
        #endregion
    }
}
