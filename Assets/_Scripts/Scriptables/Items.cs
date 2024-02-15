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

        [SerializeField] private float weight;
        [SerializeField] private string itemName;
        [SerializeField] private string description;
        [SerializeField] private float cost;
        [SerializeField] private float sellCost;
        [SerializeField] private ItemRarity rarity;
        [SerializeField] private Sprite image;
        [SerializeField] private GameObject model;

        #endregion

        #region Properties

        public float Weight => weight;
        public float Cost => cost;
        public float SellCost => sellCost;
        public ItemRarity Rarity => rarity;
        public Sprite ItemImage => image;
        public GameObject Model => model;
        public string ItemName => itemName;
        public string Description => description;

        #endregion
        
    }
}
