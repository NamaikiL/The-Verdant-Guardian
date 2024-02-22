using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Gameplay
{
    
    /**
     * <summary>
     * Handler of the NPC.
     * </summary>
     */
    [RequireComponent(typeof(SphereCollider))]
    public class NpcController : MonoBehaviour
    {
        #region Variables

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
        void Start()
        {
            InitializeSphereCollider();
        }
    

        /**
         * <summary>
         * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        void OnTriggerEnter(Collider other)
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
        void OnTriggerExit(Collider other)
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
        private void InitializeSphereCollider()
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
        private void GiveQuest(PlayerController playerController)
        {
            playerController.ReceiveNewQuest(quest);
        }

        #endregion
    }
}
