using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        #region Variables
        
        [Header("Conditions")]
        [SerializeField] private bool isActive;
        
        [Header("Main Information")]
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private List<Objectives> objectives = new List<Objectives>();
        
        // EVENT
        public static event Action<Quest> OnQuestComplete; 

        #endregion

        #region Properties

        // CONDITION
        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        // MAIN INFORMATION
        public string Title => title;
        public string Description => description;
        public List<Objectives> Objectives => objectives;

        #endregion
        
        #region Custom Methods

        /**
         * <summary>
         * Updated every time a quest objective is updated.
         * </summary>
         */
        public void TryToEndQuest()
        {
            Debug.Log("J'ai essayé");
        }

        #endregion"
    }
}
