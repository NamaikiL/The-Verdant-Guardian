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
        [Header("Inventory Properties")]
        [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();
        [SerializeField] private int inventoryItemCapacity = 25;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public int InventoryItemCapacity => inventoryItemCapacity;

        public void InitializeInventory()
        {
            for (int i = 0; i < inventoryItemCapacity; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(Items itemScriptable, int quantity)
        {
            if(!itemScriptable.IsStackable)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(quantity > 0 && !IsInventoryFull())
                    {
                        quantity -= AddItemToFirstFreeSlot(itemScriptable, 1);
                    }
                    InformAboutChange();
                    return quantity;
                }
            }

            quantity = AddStackableItem(itemScriptable, quantity);
            InformAboutChange();
            return quantity;
        }
        
        public int AddItem(Item item, Items itemScriptable, ItemType itemType, int quantity, PlayerController playerController)
        {
            if(!itemScriptable.IsStackable)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(quantity > 0 && !IsInventoryFull())
                    {
                        if (item.IsQuestConnected)
                        {
                            // If the item is a quest item.
                            foreach (Quest quest in playerController.PlayerQuestsList)
                            {
                                // If one of the quest the player have has the item required in its objectives.
                                foreach (Objectives objective in quest.QuestObjectives)
                                {
                                    if (!objective.IsComplete
                                        && objective.ActualObjectiveType == ObjectiveType.Collect
                                        && objective.ActualItemType == itemType)
                                    {
                                        // Check the objective condtions.
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

                        quantity -= AddItemToFirstFreeSlot(itemScriptable, 1);
                    }
                    InformAboutChange();
                    return quantity;
                }
            }

            quantity = AddStackableItem(itemScriptable, quantity);
            InformAboutChange();
            return quantity;
        }


        public void RemoveItemFromInventory(int itemIndex)
        {
            if (inventoryItems.Count > itemIndex)
            {
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                InformAboutChange();
            }
        }
        

        private int AddStackableItem(Items itemScriptable, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;
                if (inventoryItems[i].item.ItemId == itemScriptable.ItemId)
                {
                    int amountPossibleToTake = inventoryItems[i].item.MaxStack - inventoryItems[i].quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStack);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }

            while (quantity > 0 && !IsInventoryFull())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, itemScriptable.MaxStack);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(itemScriptable, newQuantity);
            }

            return quantity;
        }

        private int AddItemToFirstFreeSlot(Items itemScriptable, int quantity)
        {
            InventoryItem inventoryItem = new InventoryItem
            {
                item = itemScriptable,
                quantity = quantity
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = inventoryItem;
                    return quantity;
                }
            }

            return 0;
        }

        private bool IsInventoryFull()
            => inventoryItems.Any(item => item.IsEmpty) == false;

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

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            (inventoryItems[itemIndex1], inventoryItems[itemIndex2]) = (inventoryItems[itemIndex2], inventoryItems[itemIndex1]);
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
    }

    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public Items item;
        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int itemQuantity)
        {
            return new InventoryItem
            {
                item = item,
                quantity = itemQuantity,
            };
        }

        public static InventoryItem GetEmptyItem()
            => new InventoryItem
            {
                item = null,
                quantity = 0,
            };
    }
}
