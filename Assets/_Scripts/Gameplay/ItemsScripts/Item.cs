using _Scripts.Gameplay.CharactersController.Player;
using _Scripts.Managers;
using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Gameplay.ItemsScripts
{
    /**
     * <summary>
     * Item types.
     * </summary>
     */
    public enum ItemType
    {
        Collectable,
        Weapon,
        Armor,
        Consumable
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
        [SerializeField] private int itemQuantity;
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
        protected Items ItemScriptable => itemScriptable;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        protected virtual void Start()
        {
            // Component.
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
            if (other.CompareTag("Player"))
            {    // If the player go into the item zone.
                if (interactionUI)
                {   // If the item can be interact with.
                    interactionUI.SetActive(true);
                    //EVENT
                    PlayerController.OnInteraction += CollectItem;      // Subscribe to the OnInteraction Event.
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
        {   // If the UI is still active.
            if(interactionUI)
            {   // If the item can be interact with.
                // Stop the interaction.
                interactionUI.SetActive(false);
                PlayerController.OnInteraction -= CollectItem;      // UnSubscribe to the OnInteractionEvent.
            }       
        }       

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
            int quantityReminder = _inventoryManager.InventoryScriptable.AddItem(
                                    this,
                                    itemScriptable,
                                    itemType,
                                    itemQuantity,
                                    playerController
                                    ); // Add items to the player inventory.
            
            if (quantityReminder == 0) Destroy(gameObject);
            else itemQuantity = quantityReminder;
        }

        #endregion
    
    }
}