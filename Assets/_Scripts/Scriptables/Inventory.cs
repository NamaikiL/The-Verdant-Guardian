using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay;
using UnityEngine;

namespace _Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "RPG/Inventory", order = 0)]
    public class Inventory : ScriptableObject
    {
        #region Variables

        [Header("Inventory Properties")]
        [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();
        [SerializeField] private int inventoryItemCapacity = 25;

        // Event.
        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        #endregion

        #region Initialize Methods

        /**
         * <summary>
         * Initialize the inventory.
         * </summary>
         */
        public void InitializeInventory()
        {
            for (int i = 0; i < inventoryItemCapacity; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        #endregion

        #region Inventory Slot Methods

        /**
         * <summary>
         * Add an item to the inventory if the inventory isn't full (Simple version).
         * </summary>
         * <param name="itemSO">The item data.</param>
         * <param name="itemQuantity">The item quantity.</param>
         * <returns>The int value of the quantity remaining.</returns>
         */
        public int AddItem(Items itemSO, int itemQuantity)
        {
            if(!itemSO.IsStackable)
            {   // If the item is not a stackable item.
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(itemQuantity > 0 && !IsInventoryFull())
                    {   // While inventory not full and item quantity is more than 0.
                        itemQuantity -= AddItemToFirstFreeSlot(itemSO, 1);  // Add the non-stackable item.
                    }
                    InformAboutChange();    // Inform the UI about the changes.
                    return itemQuantity;
                }
            }

            itemQuantity = AddStackableItem(itemSO, itemQuantity);      // Add the stackable item.
            InformAboutChange(); // Inform the UI about the changes.
            return itemQuantity;
        }
        
        
        /**
         * <summary>
         * Add an item to the inventory if the inventory isn't full (Quest version). 
         * </summary>
         * <param name="itemScript">The script of the item.</param>
         * <param name="itemSO">The item data.</param>
         * <param name="itemType">The item type.</param>
         * <param name="itemQuantity">The quantity of the item.</param>
         * <param name="playerController">The player controller.</param>
         * <returns>The int value of the quantity remaining.</returns>
         */
        public int AddItem(Item itemScript, Items itemSO, ItemType itemType, int itemQuantity, PlayerController playerController)
        {
            if(!itemSO.IsStackable)
            {   // If the item is not a stackable item.
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(itemQuantity > 0 && !IsInventoryFull())
                    {   // While inventory not full and item quantity is more than 0.
                        if (itemScript.IsQuestConnected)
                        {   // If the item is a quest item.
                            foreach (Quest quest in playerController.PlayerQuestsList)
                            {   // If one of the quest the player have has the item required in its objectives.
                                foreach (Objectives objective in quest.QuestObjectives)
                                {
                                    if (!objective.IsComplete
                                        && objective.ActualObjectiveType == ObjectiveType.Collect
                                        && objective.ActualItemType == itemType)
                                    {
                                        // Check the objective conditions.
                                        objective.NbCollected++;

                                        if (objective.NbCollected == objective.NbToCollect)
                                        {
                                            // When the number to collect is the one required.
                                            objective.CompleteObjective();
                                        }
                                    }
                                }
                            }
                        }

                        itemQuantity -= AddItemToFirstFreeSlot(itemSO, 1);  // Add the non-stackable item.
                    }
                    InformAboutChange();    // Inform the UI about the changes.
                    return itemQuantity;
                }
            }

            itemQuantity = AddStackableItem(itemSO, itemQuantity);  // Add the stackable item.
            InformAboutChange();    // Inform the UI about the changes.
            return itemQuantity;
        }

        
        /**
         * <summary>
         * Remove the items from a slot.
         * </summary>
         * <param name="itemInventoySlotIndex">The id of the inventory slot.</param>
         */
        public void RemoveItemFromInventory(int itemInventoySlotIndex)
        {
            if (inventoryItems.Count > itemInventoySlotIndex)
            {   // Check if the item slot exist.
                inventoryItems[itemInventoySlotIndex] = InventoryItem.GetEmptyItem();   // Reset the slot.
                InformAboutChange();    // Inform the UI about the change.
            }
        }
        

        /**
         * <summary>
         * Add stackable items to the inventory.
         * </summary>
         * <param name="itemSO">The item data.</param>
         * <param name="itemQuantity">The item quantity.</param>
         * <returns>Return the value of the item quantity remaining</returns>
         */
        private int AddStackableItem(Items itemSO, int itemQuantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;    // Skip the slot if the inventory slot is empty.
                if (inventoryItems[i].item.ItemId == itemSO.ItemId)
                {   // If the item id of the slot is the same item as the one on the parameters
                    int amountPossibleToTake = inventoryItems[i].item.MaxStack - inventoryItems[i].quantity;    // Calculate the amount it can take.

                    if (itemQuantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStack);
                        itemQuantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + itemQuantity);
                        InformAboutChange();    // Inform the UI about the changes.
                        return 0;
                    }
                }
            }

            while (itemQuantity > 0 && !IsInventoryFull())
            {       // If the item isn't in the inventory yet.
                int newQuantity = Mathf.Clamp(itemQuantity, 0, itemSO.MaxStack);
                itemQuantity -= newQuantity;
                AddItemToFirstFreeSlot(itemSO, newQuantity);
            }

            return itemQuantity;
        }

        
        /**
         * <summary>
         * Add the item to the first free inventory slot.
         * </summary>
         * <param name="itemSO">The item data.</param>
         * <param name="itemQuantity">The item quantity.</param>
         * <returns>Return the value of the item quantity remaining</returns>
         */
        private int AddItemToFirstFreeSlot(Items itemSO, int itemQuantity)
        {
            // Define the inventory slot item values.
            InventoryItem inventoryItem = new InventoryItem
            {
                item = itemSO,
                quantity = itemQuantity
            };

            // Check if there's a free slot.
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = inventoryItem;
                    return itemQuantity;
                }
            }

            return 0;
        }

        #endregion

        #region Conditions Methods

        /**
         * <returns>Returns a bool if the inventory is full or not.</returns>
         */
        private bool IsInventoryFull()
            => inventoryItems.Any(item => item.IsEmpty) == false;

        
        /**
         * <summary>
         * Get the current Inventory State.
         * </summary>
         * <returns>Returns a dictionary of the inventory.</returns>
         */
        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                returnValue[i] = inventoryItems[i];
            }

            return returnValue;
        }

        #endregion

        #region UI Methods

        /**
         * <summary>
         * Get the inventory item from the index given.
         * </summary>
         * <param name="itemInventorySlotIndex">The index of the inventory slot.</param>
         */
        public InventoryItem GetItemAt(int itemInventorySlotIndex)
        {
            return inventoryItems[itemInventorySlotIndex];
        }

        
        /**
         * <summary>
         * Swap two item in the inventory.
         * </summary>
         * <param name="itemInventorySlotIndex1">The first item.</param>
         * <param name="itemInventorySlotIndex2">The second item.</param>
         */
        public void SwapItems(int itemInventorySlotIndex1, int itemInventorySlotIndex2)
        {
            (inventoryItems[itemInventorySlotIndex1], inventoryItems[itemInventorySlotIndex2]) = 
            (inventoryItems[itemInventorySlotIndex2], inventoryItems[itemInventorySlotIndex1]);
            InformAboutChange();    // Inform the UI about the changes.
        }

        
        /**
         * <summary>
         * Inform the UI about the changes.
         * </summary>
         */
        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        #endregion
    }

    
    /**
     * <summary>
     * Represent the items.
     * </summary>
     */
    [Serializable]
    public struct InventoryItem
    {
        #region Variables

        public int quantity;
        public Items item;

        #endregion

        #region Properties

        public bool IsEmpty => item == null;

        #endregion

        #region Constructors Methods

        /**
         * <summary>
         * Initialize the Inventory Item.
         * </summary>
         */
        public InventoryItem ChangeQuantity(int itemQuantity)
        {
            return new InventoryItem
            {
                item = item,
                quantity = itemQuantity,
            };
        }

        
        /**
         * <summary>
         * Empty the inventory item.
         * </summary>
         */
        public static InventoryItem GetEmptyItem()
            => new InventoryItem
            {
                item = null,
                quantity = 0,
            };

        #endregion
    }
}
