using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Gameplay
{
    /**
     * <summary>
     * Item types.
     * </summary>
     */
    public enum ItemType
    {
        Collectable,
        Weapon
    }

    [RequireComponent(typeof(SphereCollider))]
    public class Item : MonoBehaviour
    {
        #region Variables

        [Header("Quest Parameters")]
        [SerializeField] private bool isQuestConnected;
        
        [Header("Item Properties")]
        [SerializeField] private ItemType itemType;
        [SerializeField] protected Items scriptable;
        
        [Header("UI Properties")]
        [SerializeField] private GameObject interactionUI;

        #endregion

        #region Properties
        
        // Quest Parameters.
        public bool IsQuestConnected => isQuestConnected;
        
        // Item Properties.
        public Items Scriptable => scriptable;

        #endregion

        #region Built-In Methods
    
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
            playerController.AddItemToInventory(this, scriptable, itemType);
        }

        #endregion
    
    }
}