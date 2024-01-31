using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        #region Variables

        public static event Action<Quest> OnQuestComplete; 
        
        public bool isActive;
        public string title;
        public string description;
        public List<Objectives> objectives = new List<Objectives>();

        #endregion

        #region Methods

        public void TryToEndQuest()
        {
            Debug.Log("J'ai essayé");
        }

        #endregion"
    }
}
