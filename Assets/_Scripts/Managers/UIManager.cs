using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region Variables

    [SerializeField] private Transform panQuestHolder;
    [SerializeField] private GameObject panQuest;

    private static UIManager _instance;

    #endregion

    #region Properties

    public static UIManager Instance => _instance;

    #endregion

    #region Built-In Methods

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
     */
    public void AddNewQuest(string title, string description)
    {
        GameObject quest = Instantiate(panQuest, panQuestHolder);
        quest.transform.GetChild(0).GetComponent<TMP_Text>().text = title;
        quest.transform.GetChild(1).GetComponent<TMP_Text>().text = description;
    }

    #endregion

}
