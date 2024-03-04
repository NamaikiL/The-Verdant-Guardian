using System.Globalization;
using _Scripts.Scriptables;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ItemTooltip : MonoBehaviour
    {
        #region Variables

        [Header("Items Properties")] 
        [SerializeField] private TMP_Text txtName;
        [SerializeField] private TMP_Text txtDesc;
        [SerializeField] private TMP_Text txtStat;
        [SerializeField] private TMP_Text txtDurability;
        [SerializeField] private TMP_Text txtCost;
        [SerializeField] private TMP_Text txtRarity;

        #endregion

        #region Initializing Methods

        public void InitializeTooltip(Items itemSO)
        {
            txtName.text = itemSO.ItemName;
            txtDesc.text = itemSO.ItemDescription;

            if (itemSO is Weapons weaponSO)
            {
                txtStat.text = $"{weaponSO.Damage} Atk | {weaponSO.AttackSpeed} Atk Spd";
                txtDurability.text = weaponSO.Durability.ToString(CultureInfo.CurrentCulture);
            }

            txtCost.text = itemSO.ItemCost.ToString(CultureInfo.CurrentCulture);
            txtRarity.text = itemSO.ItemRarity.ToString();
        }


        public void ResetToolTip()
        {
            txtName.text = "";
            txtDesc.text = "";
            txtStat.text = "";
            txtDurability.text = "";
            txtCost.text = "";
            txtRarity.text = "";
        }

        #endregion
    }
}
