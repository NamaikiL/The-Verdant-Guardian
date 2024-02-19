using System;
using UnityEngine;

namespace _Scripts.Scriptables
{
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
    
    public class Items : ScriptableObject
    {
        #region Variables

        [Header("Item Properties")]
        // Item Presentation.
        [SerializeField] private string itemName;
        [SerializeField] private string itemDescription;
        // Item Information
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

        public float ItemWeight => itemWeight;
        public float ItemCost => itemCost;
        public float ItemSellCost => itemSellCost;
        public ItemRarity ItemRarity => itemRarity;
        public Sprite ItemImage => itemImage;
        public GameObject ItemModel => itemModel;
        public string ItemName => itemName;
        public string ItemDescription => itemDescription;

        #endregion
        
    }
}
