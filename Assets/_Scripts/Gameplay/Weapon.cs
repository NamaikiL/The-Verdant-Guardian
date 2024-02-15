using System;
using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class Weapon : Item
    {
        #region Variables
        
        private int _damage;
        private float _attackSpeed;
        private float _weight;
        private float _cost;
        private float _sellCost;
        private float _durability;
        private ItemRarity _rarity;

        #endregion

        #region Properties
        
        // STATS
        public int Damage => _damage;

        public float AttackSpeed => _attackSpeed;
        
        public float Weight => _weight;
        
        public float Cost => _cost;
        
        public float SellCost => _sellCost;
        
        public float Durability => _durability;
        
        public ItemRarity Rarity => _rarity;

        #endregion

        void OnEnable()
        {
            //_damage = scriptable.Damage;
            //_attackSpeed = scriptable.AttackSpeed;
            _weight = scriptable.Weight;
            _cost = scriptable.Cost;
            _sellCost = scriptable.SellCost;
            _rarity = scriptable.Rarity;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
