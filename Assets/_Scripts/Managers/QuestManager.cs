using System.Collections.Generic;
using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Managers
{
    public class QuestManager : MonoBehaviour
    {
        #region Variables

        /*[Header("Quest Handler")]
        [SerializeField] private List<Quest> _playerQuestsList = new List<Quest>();

        //Quest Handler
        private Quest _playerActiveQuest;

        //Components.
        private UIManager _uiManager;

        //Singleton.
        private static QuestManager _instance;

        #endregion

        #region Properties

        // Quest Property.
        public List<Quest> PlayerQuestsList => _playerQuestsList;

        //Singleton.
        public static QuestManager Instance => _instance;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Unity calls Awake when an enabled script instance is being loaded.
         * </summary>
         */
        /*void Awake()
        {
            // Singleton.
            if (_instance) Destroy(gameObject);
            _instance = this;
        }*/

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        /*void Start ()
        {
            //Components.
         _uiManager = UIManager.Instance;
        }

        #endregion

        #region Quest Management

        /**
         * <summary>
         * Add the player a new quest.
         * </summary>
         * <param name="quest">The quest to add.</param>
         */
        /*blic void ReceiveNewQuest(Quest quest)
        {
            _playerQuestsList.Add(quest);      // Add the quest to the list.

            // Change active quest.
            quest.IsActive = true;
            _playerActiveQuest = quest;

            // EVENT.
            quest.OnQuestComplete += RemoveCompletedQuest;

            // UI.
            _uiManager.AddNewQuest(quest.QuestTitle, quest.QuestDescription);
        }
        */

        /**
         * <summary>
         * Remove the quest from the player.
         * </summary>
         * <param name="quest">The quest to remove.</param>
         */
        /*ivate void RemoveCompletedQuest(Quest quest)
        {
            // EVENT.
            quest.OnQuestComplete -= RemoveCompletedQuest;
            _playerQuestsList.Remove(quest);

            if (_playerQuestsList.Count > 0)
            {
                _playerActiveQuest = _playerQuestsList[0];
                _uiManager.AddNewQuest(_playerQuestsList[0].QuestTitle, _playerQuestsList[0].QuestDescription);
            }
            _uiManager.RemoveQuest();

            Debug.Log(quest.QuestTitle + "Quête terminé");
        }*/

        #endregion
    }
}