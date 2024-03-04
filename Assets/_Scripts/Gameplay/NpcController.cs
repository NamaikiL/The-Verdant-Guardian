using _Scripts.Managers;
using _Scripts.Scriptables;
using _Scripts.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay
{
    
    /**
     * <summary>
     * Handler of the NPC.
     * </summary>
     */
    //[RequireComponent(typeof(SphereCollider))]
    public class NpcController : MonoBehaviour
    {
        #region Variables

        [Header("Scripts")]
        [SerializeField] private HealthBar healthBar;

        [Header("NPC Health")]
        [SerializeField] private int maxNpcHP = 100;

        //NPC Stats
        private int _currentNpcHP;
        private int _damage;
        
        public Weapon weapons;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start ()
        {
            _currentNpcHP = maxNpcHP;
            _damage = weapons.WeaponDamage;

            //Update the maximum size of gauges
            healthBar.SetHealthBarMax(maxNpcHP);
        }

        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            _currentNpcHP = Mathf.Clamp(_currentNpcHP, 0, maxNpcHP);
        }

        /**
         * <summary>
         * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        void OnTriggerEnter(Collider other)
        {
            if(other.transform.tag == "Weapon")
            {
                Debug.Log("dd");
                TakeDamage(_damage);
            }
        }

        #endregion

        #region Health Management

        /**
         * <summary>
         * Remove HP from the NPC based on a quantity given.
         * </summary>
         * <param name="damage">The number of damage the player took.</param>
         */
        public void TakeDamage(int damage)
        {
            _currentNpcHP -= damage;
            healthBar.UpdateHealthBar(_currentNpcHP);

            if(_currentNpcHP == 0)
            {
                NpcDeath();
            }
        }

        /**
         * <summary>
         * Give the behaviour to the object when he dies.
         * </summary>
         */
        private void NpcDeath()
        {
            Destroy(this.gameObject);
        }

        #endregion

        /*#region Variables

        [Header("Interaction")] 
        [SerializeField] private GameObject interactionUI;
        [SerializeField] private Quest quest;
    
        // Interaction.
        private SphereCollider _trigger;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        /*void Start()
        {
            InitializeSphereCollider();
        }
    

        /**
         * <summary>
         * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        /*void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))      // If player is near.
                if(interactionUI)
                {
                    interactionUI.SetActive(true);
                    //EVENT
                    PlayerController.OnInteraction += GiveQuest;
                }       // If NPC can interact.
        }


        /**
         * <summary>
         * OnTriggerExit is called when the Collider other has stopped touching the trigger.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        /*void OnTriggerExit(Collider other)
        {
            if(interactionUI)
            {
                interactionUI.SetActive(false);
                PlayerController.OnInteraction -= GiveQuest;
            }       // If NPC can interact.
        
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Initialize the sphere collider of the NPC.
         * </summary>
         */
        /*private void InitializeSphereCollider()
        {
            _trigger = GetComponent<SphereCollider>();
            _trigger.isTrigger = true;
            _trigger.radius = 1.8f;
        }


        /**
         * <summary>
         * Give a quest.
         * </summary>
         * <param name="playerController">The player controller.</param>
         */
        /*private void GiveQuest(PlayerController playerController)
        {
            playerController.ReceiveNewQuest(quest);
        }

        #endregion*/
    }
}