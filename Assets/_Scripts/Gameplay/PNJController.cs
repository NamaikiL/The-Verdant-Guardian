using _Scripts.Gameplay;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PNJController : MonoBehaviour
{
    #region Variables

    [Header("Interaction")] 
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private Quest quest;
    
    private SphereCollider _trigger;

    #endregion

    #region Built-In Methods

    /**
     * <summary>
     * Start is called before the first frame update.
     * </summary>
     */
    void Start()
    {
        InitializeSphereCollider();
    }

    
    /**
     * <summary>
     * Update is called once per frame.
     * </summary>
     */
    void Update()
    {
        
    }
    

    /**
     * <summary>
     * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
     * </summary>
     * <param name="other">The other Collider involved in this collision.</param>
     */
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            if(interactionUI)
            {
                interactionUI.SetActive(true);
                //EVENT
                PlayerController.OnInteraction += GiveQuest;
            }
    }


    /**
     * <summary>
     * OnTriggerExit is called when the Collider other has stopped touching the trigger.
     * </summary>
     * <param name="other">The other Collider involved in this collision.</param>
     */
    void OnTriggerExit(Collider other)
    {
        if(interactionUI)
        {
            interactionUI.SetActive(false);
            PlayerController.OnInteraction -= GiveQuest;
        }
        
    }

    #endregion

    #region Custom Methods

    /**
     * <summary>
     * Initialize the sphere collider of the NPC.
     * </summary>
     */
    private void InitializeSphereCollider()
    {
        _trigger = GetComponent<SphereCollider>();
        _trigger.isTrigger = true;
        _trigger.radius = 1.8f;
    }


    private void GiveQuest(PlayerController controller)
    {
        controller.ReceiveNewQuest(quest);
    }

    #endregion
}
