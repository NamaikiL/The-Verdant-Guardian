using UnityEngine;

namespace _Scripts.Gameplay
{
    public enum ItemType
    {
        Apple
    }

    [RequireComponent(typeof(SphereCollider))]
    public class Item : MonoBehaviour
    {
        #region Variables

        [Header("Item Properties")]
        [SerializeField] private ItemType itemType;
        
        [Header("UI Properties")]
        [SerializeField] private GameObject interactionUI;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        void Start()
        {
        
        }

    
        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
        
        }
    
    
        /**
         * <summary>
         * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))  // If the player go into the item zone.
                if(interactionUI)
                {
                    interactionUI.SetActive(true);
                    //EVENT
                    PlayerController.OnInteraction += CollectItem;
                } // If the item can be interact with.
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
                // Stop the interaction.
                interactionUI.SetActive(false);
                PlayerController.OnInteraction -= CollectItem;
            }   // If the item can be interact with.
        }

        
        /**
         * <summary>
         * This function is called when the behaviour becomes disabled.
         * </summary>
         */
        private void OnDisable()
        {
            PlayerController.OnInteraction -= CollectItem;
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Add item to the quest objectives.
         * </summary>
         * <param name="playerController">The player controller.</param>
         */
        private void CollectItem(PlayerController playerController)
        {
            // Add items to the player inventory.
            playerController.AddItemToInventory(itemType);
            Destroy(gameObject);
        }

        #endregion
    
    }
}