using System;
using System.Collections.Generic;
using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        #region Variables

        // EVENT.
        public event Action<Quest> OnQuestComplete; 
        
        [Header("Quest Conditions")]
        [SerializeField] private bool isActive;
        [SerializeField] private bool isComplete;
        
        [Header("Quest Information")]
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private List<Objectives> objectives = new List<Objectives>();

        #endregion

        #region Properties

        // Conditions
        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        // Information
        public string Title => title;
        public string Description => description;
        public List<Objectives> Objectives => objectives;

        #endregion

        #region Quest Handler

        /**
         * <summary>
         * Quest ending handler.
         * </summary>
         */
        public void TryToEndQuest()
        {
            foreach (Objectives objective in objectives)
            {
                if (!objective.IsComplete && objective.IsRequired)
                {
                    return;
                }
                isComplete = true;
                isActive = false;
                OnQuestComplete?.Invoke(this);
            }
        }
        
        
        private void OnEnable()
        {
            foreach (Objectives objective in objectives)
            {
                objective.parentQuest = this;
            }
        }

        #endregion"
    }
}
