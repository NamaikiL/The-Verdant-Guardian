using _Scripts.Scriptables;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    /**
     * <summary>
     * The tooltip when hovering an item.
     * </summary>
     */
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

        /**
         * <summary>
         * Initialize the Tooltip.
         * </summary>
         * <param name="itemSO">the item data.</param>
         */
        public void InitializeTooltip(Items itemSO)
        {
            txtName.text = itemSO.ItemName;
            txtDesc.text = itemSO.ItemDescription;

            if (itemSO is Weapons weaponSO)
            {
                txtStat.text = $"{weaponSO.WeaponDamage} Atk | {weaponSO.WeaponAttackSpeed} Atk Spd";
                txtDurability.text = $"{weaponSO.WeaponDurability}";
            }
            else if (itemSO is Armors armorSO)
            {
                txtStat.text = $"{armorSO.ArmorDefense} DF";
                txtDurability.text = $"{armorSO.ArmorDurability}";
            }

            txtCost.text = $"{itemSO.ItemCost}";
            txtRarity.text = $"{itemSO.ItemRarity}";
        }


        /**
         * <summary>
         * Reset the Tooltip values.
         * </summary>
         */
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
