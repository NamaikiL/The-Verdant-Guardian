using TMPro;
using UnityEngine;

namespace _Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {

        #region Variables

        [Header("Quest UI.")]
        [SerializeField] private Transform panQuestHolder;
        [SerializeField] private GameObject panQuest;

        // Singleton.
        private static UIManager _instance;

        #endregion

        #region Properties

        // Singleton.
        public static UIManager Instance => _instance;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Awake is called when an enabled script instance is being loaded.
         * </summary>
         */
        void Awake()
        {
            if (_instance) Destroy(this);
            _instance = this;
        }


        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        void Start()
        {
        
        }

    
        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
        
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Add quest to the quest holder.
         * </summary>
         * <param name="title">The title of the quest.</param>
         * <param name="description">The description of the quest.</param>
         */
        public void AddNewQuest(string title, string description)
        {
            GameObject quest = Instantiate(panQuest, panQuestHolder);
            quest.transform.GetChild(0).GetComponent<TMP_Text>().text = title;
            quest.transform.GetChild(1).GetComponent<TMP_Text>().text = description;
        }

        #endregion

    }
}
