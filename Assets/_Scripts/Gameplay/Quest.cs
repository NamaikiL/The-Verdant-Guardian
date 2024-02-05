using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        #region Variables

        // EVENT.
        public static event Action<Quest> OnQuestComplete; 
        
        [Header("Quest Conditions")]
        [SerializeField] private bool isActive;
        
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
            Debug.Log("J'ai essayé");
        }

        #endregion"
    }
}
