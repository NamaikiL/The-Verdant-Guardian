using System.Collections.Generic;
using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts
{
    public class InstantiateRandomWeapon : MonoBehaviour
    {
        #region Variables

        // List of weapons to spawn.
        [SerializeField] private List<Weapons> weapons = new List<Weapons>();

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        void Start()
        {
            int rndWeapon = Random.Range(0, weapons.Count);     // Random Weapon.
            
            // GameObject
            GameObject weapon = Instantiate(
                weapons[rndWeapon].ItemModel, 
                transform.position,
                Quaternion.identity);
            
            weapon.name = weapons[rndWeapon].name;
        }

        #endregion
        
    }
}
