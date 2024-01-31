using System;
using _Scripts.Gameplay;
using UnityEngine;

public enum ItemType
{
    Apple
}

[RequireComponent(typeof(SphereCollider))]
public class Item : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject interactionUI;
    [SerializeField] private ItemType itemType;

    #endregion

    #region Built-In Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    // Update is called once per frame
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
                PlayerController.OnInteraction += CollectItem;
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
            PlayerController.OnInteraction -= CollectItem;
        }
    }

    private void OnDisable()
    {
        PlayerController.OnInteraction -= CollectItem;
    }

    #endregion

    #region Custom Methods

    private void CollectItem(PlayerController controller)
    {
        controller.AddItemToInventory(itemType);
        Destroy(gameObject);
    }

    #endregion
    
}
