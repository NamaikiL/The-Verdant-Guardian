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
        [SerializeField] private AudioSource btnSFX;
        [SerializeField] private AudioSource questCompletSFX;

        [Header("Player SFX")]
        [SerializeField] private AudioSource playerHealedSFX;
        [SerializeField] private AudioSource playerDyingSFX;
        [SerializeField] private AudioSource playerDeathSFX;
        [SerializeField] private AudioSource playerAttackSFX;
        [SerializeField] private AudioSource playerSleepSFX;

        [Header("Enemies/NPC SFX")]
        [SerializeField] private AudioSource enemyDeathSFX;
        [SerializeField] private AudioSource[] enemyIdleSFX;
        [SerializeField] private float minRadomTimeIdleSFX = 5f;
        [SerializeField] private float maxRadomTimeIdleSFX = 30f;
        [SerializeField] private AudioSource enemyAttackSFX;

        //Singleton.
        private static AudioManager _instance;

        #endregion

        #region Properties

        //User Interface SFX.
        public AudioSource BtnSFX => btnSFX;
        public AudioSource QuestCompletSFX => questCompletSFX;

        //Player SFX.
        public AudioSource PlayerHealedSFX => playerHealedSFX;
        public AudioSource PlayerDyingSFX => playerDyingSFX;
        public AudioSource PlayerDeathSFX => playerDeathSFX;
        public AudioSource PlayerAttackSFX => playerAttackSFX;
        public AudioSource PlayerSleepSFX => playerSleepSFX;

        //Ennemies/NPC SFX.
        public AudioSource EnemyDeathSFX => enemyDeathSFX;
        public AudioSource[] EnemyIdleSFX => enemyIdleSFX;
        public float MinRadomTimeIdleSFX => minRadomTimeIdleSFX;
        public float MaxRadomTimeIdleSFX => maxRadomTimeIdleSFX;
        public AudioSource EnemyAttackSFX => enemyAttackSFX;

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
