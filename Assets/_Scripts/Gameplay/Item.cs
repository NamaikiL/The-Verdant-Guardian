using _Scripts.Managers;
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
    
    
    /**
     * <summary>
     * The base Item script.
     * </summary>
     */
    [RequireComponent(typeof(SphereCollider))]
    public class Item : MonoBehaviour
    {
        #region Variables

        [Header("Quest Parameters")]
        [SerializeField] private bool isQuestConnected;
        
        [Header("Scriptable Data")]
        [SerializeField] protected Items itemScriptable;
        
        [Header("Item Properties")]
        [SerializeField] private ItemType itemType;
        
        [Header("UI Properties")]
        [SerializeField] private GameObject interactionUI;
        
        // Components.
        private InventoryManager _inventoryManager;
        
        #endregion

        #region Properties
        
        // Quest Conditions.
        public bool IsQuestConnected => isQuestConnected;
        
        // Item Properties.
        public Items ItemScriptable => itemScriptable;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        protected virtual void Start()
        {
            _inventoryManager = InventoryManager.Instance;
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
        
        
        /**
         * <summary>
         * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")){ 
                if (interactionUI)
                {
                    interactionUI.SetActive(true);
                    //EVENT
                    PlayerController.OnInteraction += CollectItem;      // Subscribe to the OnInteraction Event.
                }       // If the item can be interact with.
            }       // If the player go into the item zone.
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
                PlayerController.OnInteraction -= CollectItem;      // UnSubscribe to the OnInteractionEvent.
            }       // If the item can be interact with.
        }       // If the UI is still active.

        #endregion

        #region Item Behavior Methods

        /**
         * <summary>
         * Add item to the quest objectives.
         * </summary>
         * <param name="playerController">The player controller.</param>
         */
        private void CollectItem(PlayerController playerController)
        {
            Debug.Log(_inventoryManager);
            _inventoryManager.AddItemToInventory(this, itemScriptable, itemType, playerController);        // Add items to the player inventory.
        }

        #endregion
    
    }
}