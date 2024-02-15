using System.Collections.Generic;
using _Scripts.Gameplay;
using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts
{
    public class InstantiateRandomWeapon : MonoBehaviour
    {
        #region Variables

        [SerializeField] private List<Weapons> weapons = new List<Weapons>();

        #endregion
        
        // Start is called before the first frame update
        void Start()
        {
            int rndWeapon = Random.Range(0, weapons.Count);     // Random Weapon.
            
            // GameObject
            GameObject weapon = Instantiate(
                weapons[rndWeapon].Model, 
                transform.position,
                Quaternion.identity);
            
            weapon.name = weapons[rndWeapon].name;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
