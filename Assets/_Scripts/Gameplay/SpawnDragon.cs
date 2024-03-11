using _Scripts.Scriptables;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class SpawnDragon : MonoBehaviour
    {
        #region Variables

        [Header("Spawn Properties")] 
        [SerializeField] private GameObject dragonPrefab;
        [SerializeField] private Transform spawnPoint;

        #endregion

        #region Spawn Methods

        /**
         * <summary>
         * Spawn the dragon if the conditions are complete.
         * </summary>
         */
        public void Spawn()
        {
            Quest[] quests = Resources.LoadAll<Quest>("Quests/MainQuest");
            
            foreach (Quest quest in quests)
            {
                if (!quest.IsComplete
                    || quest.name == "Quest_KillDragon" && quest.IsComplete) return;
            }

            Instantiate(dragonPrefab, spawnPoint.position, quaternion.identity);
        }

        #endregion
    }
}
