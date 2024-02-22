using System;
using UnityEngine;

namespace _Scripts.Scriptables
{
    /**
     * <summary>
     * Rarity of the item.
     * </summary>
     */
    [Serializable]
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }
    
    
    /**
     * <summary>
     * Handler of the item and its different type in the game.
     * </summary>
     */
    public class Items : ScriptableObject
    {
        #region Variables

        [Header("Item Properties")]
        // Item Information.
        [SerializeField] private string itemName;
        [SerializeField] private string itemDescription;
        [SerializeField] private ItemRarity itemRarity;
        [SerializeField] private float itemWeight;
        // Economic variables.
        [SerializeField] private float itemCost;
        [SerializeField] private float itemSellCost;
        // Item Assets
        [SerializeField] private Sprite itemImage;
        [SerializeField] private GameObject itemModel;

        #endregion

        #region Properties

        // Item Information Properties
        public string ItemName => itemName;
        public string ItemDescription => itemDescription;
        public ItemRarity ItemRarity => itemRarity;
        public float ItemWeight => itemWeight;
        
        // Item Economic Properties.
        public float ItemCost => itemCost;
        public float ItemSellCost => itemSellCost;
        
        // Item Assets Properties.
        public Sprite ItemImage => itemImage;
        public GameObject ItemModel => itemModel;
        
        #endregion
        
    }
}
