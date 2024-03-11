using System.Collections;
using _Scripts.Gameplay.CharactersController.Player;
using _Scripts.Gameplay.EnemiesController;
using _Scripts.Managers;
using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Gameplay.CharactersController.Npc
{
    
    /**
     * <summary>
     * Handler of the NPC.
     * </summary>
     */
    //[RequireComponent(typeof(SphereCollider))]
    public class NpcController : EnemyController
    {
        #region Variables
        
        [Header("Interaction")]
        [SerializeField] private GameObject interactionUI;
        [SerializeField] private Quest quest;
        [SerializeField] private GameObject dialogue;

        // Interaction.
        private SphereCollider _trigger;

        //Idle SFX.
        private bool _isSfxPlaying;

        //Component.
        private AudioManager _audioManager;
        private PlayerInputs _playerInputs;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start ()
        {
            //Components.
            _audioManager = AudioManager.Instance;
            _playerInputs = PlayerInputs.Instance;

            InitializeSphereCollider();
            IdleSFX();
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
            {       // If player is near
                
                //NPC look at the player.
                gameObject.transform.LookAt(other.gameObject.transform);

                if (interactionUI)
                {
                    interactionUI.SetActive(true);

                    if (_playerInputs.Interaction)
                    {
                        Debug.Log("ddd");
                        NpcInteraction();
                    }
                }

                //EVENT
                PlayerController.OnInteraction += GiveQuest;
            }      // If NPC can interact.            
        }
        
        
        /**
         * <summary>
         * OnTriggerExit is called when the Collider other has stopped touching the trigger.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        void OnTriggerExit(Collider other)
        {
            if(interactionUI)
            {
                interactionUI.SetActive(false);
                gameObject.transform.LookAt(this.gameObject.transform);
                NpcStopInteraction();

                PlayerController.OnInteraction -= GiveQuest;
            }       // If NPC can interact.
        
        }

        #endregion

        #region Behaviour Methods

        /**
         * <summary>
         * Play idle SFX.
         * </summary>
         */
        private void IdleSFX()
        {
            StartCoroutine(DelayIdleSFX());
        }

        
        /**
         * <summary>
         * Coroutine to play random SFX at random time.
         * </summary>
         */
        private IEnumerator DelayIdleSFX()
        {
            _isSfxPlaying = true;

            if (_isSfxPlaying == true)
            {
                //Choose random SFX.
                AudioSource idleSFX = _audioManager.NpcIdleSFX[Random.Range(0, _audioManager.NpcIdleSFX.Length)];
                idleSFX.Play();
                _isSfxPlaying = false;

                if (_isSfxPlaying == false)
                {
                    //Play SFX at random time.
                    float randomTime = Random.Range(_audioManager.MinRadomTimeIdleSFX, _audioManager.MaxRadomTimeIdleSFX);
                    yield return new WaitForSeconds(randomTime);

                    IdleSFX();
                }   //Allows the SFX to be play more than one time.
            }
        }
        
        #endregion

        #region Quest Management

        /**
         * <summary>
         * Initialize the sphere collider of the NPC.
         * </summary>
         */
        private void InitializeSphereCollider()
        {
            _trigger = GetComponent<SphereCollider>();
            _trigger.isTrigger = true;
            _trigger.radius = 3f;
        }

        private void NpcInteraction()
        {
            GetComponent<EnemyController>().Patrol(true);
            dialogue.SetActive(true);
        }

        private void NpcStopInteraction()
        {
            GetComponent<EnemyController>().Patrol(false);
            dialogue.SetActive(false);
        }

        /**
         * <summary>
         * Give a quest.
         * </summary>
         * <param name="playerController">The player controller.</param>
         */
        private void GiveQuest(PlayerController playerController)
        {
            //playerController.ReceiveNewQuest(quest);
        }
        
        #endregion
        
    }
}