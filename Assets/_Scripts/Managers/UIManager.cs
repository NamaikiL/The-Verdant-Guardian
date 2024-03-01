using _Scripts.Scriptables;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    
    /**
     * <summary>
     * Manager of the UI in general.
     * </summary>
     */
    public class UIManager : MonoBehaviour
    {
        #region Variables
        [Header("Life gauge UI.")]
        [SerializeField] private Slider lifeSlider;
        [SerializeField] private Slider damageSlider;
        [SerializeField] private float cooldownDamageEffect = 0.3f;

        [Header("Floating life gauge position")]
        [SerializeField] private Camera cam;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 gaugePosition;

        [Header("Stamina gauge UI.")]
        [SerializeField] private Slider staminaSlider;

        [Header("Quest UI.")]
        [SerializeField] private Transform panQuestHolder;
        [SerializeField] private GameObject panQuest;
        
        [Header("Inventory")]
        // Panels.
        [SerializeField] private GameObject panInventory;
        // Items UI Assets.
        [SerializeField] private GameObject btnItem;
        // Item UI Information.
        [SerializeField] private Image imgItem;
        [SerializeField] private TMP_Text txtName;
        [SerializeField] private TMP_Text txtPrice;
        [SerializeField] private Button btnDrop;

        // Base variable for item case in inventory.
        private GameObject _itemCase;

        // Inventory Variables.
        private bool _inventoryShowed;
        
        // Components.
        private InventoryManager _inventoryManager;
        
        // Singleton.
        private static UIManager _instance;

        #endregion

        #region Properties

        // Singleton Property.
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
            // Singleton.
            if (_instance) Destroy(this);
            _instance = this;
        }


        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start()
        {
            _inventoryManager = InventoryManager.Instance;
        }

        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            LifeBarPosition();
        }

        #endregion

        #region Health Management
        /**
         * <summary>
         * Update the size of the gauge based on the maximum life of the character.
         * </summary>
         * <param name="maxLife">The maximum life disponible. </param>
         */
        public void SetLifeBarMax(int maxLife)
        {
            lifeSlider.maxValue = maxLife;
            damageSlider.maxValue = maxLife;
            UpdateLifeBar(maxLife);
        }

        /**
         * <summary>
         * Update life bar.
         * </summary>
         * <param name="currentLife">The actual life value. </param>
         */
        public void UpdateLifeBar(int currentLife)
        {
            lifeSlider.value = currentLife;
            StartCoroutine(EffectLifeDamage(currentLife));
        }

        /**
         * <summary>
         * Visual effect when a character loose life.
         * </summary>
         * <param name="currentDamage">The actual damage value. </param>
         */
        private IEnumerator EffectLifeDamage(int currentDamage)
        {
            yield return new WaitForSeconds(cooldownDamageEffect);
            damageSlider.value = currentDamage;
        }

        /**
         * <summary>
         * Set up the rotation and position of floating life bar depending on the character and the camera.
         * </summary>
         */
        private void LifeBarPosition()
        {
            if (cam)
            {
                transform.rotation = cam.transform.rotation;
            }

            if (target)
            {
                transform.position = target.position + gaugePosition;
            }
        }

        #endregion

        #region Stamina Management

        /**
         * <summary>
         * Update the size of the gauge based on the maximum stamina.
         * </summary>
         * <param name="maxStamina">The maximum stamina disponible. </param>
         */
        public void SetStaminaBarMax(float maxStamina)
        {
            staminaSlider.maxValue = maxStamina;
            UpdateStaminaBar(maxStamina);
        }

        /**
         * <summary>
         * Update stamina bar.
         * </summary>
         * <param name="currentStamina">The actual stamina value. </param>
         */
        public void UpdateStaminaBar(float currentStamina)
        {
            staminaSlider.value = currentStamina;
        }

        #endregion

        #region Quest Management

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


        /**
         * <summary>
         * Remove the active quest from the quest holder.
         * </summary>
         */
        public void RemoveQuest()
        {
            Destroy(panQuestHolder.transform.GetChild(0).gameObject);
        }

        /**
         * <summary>
         * Manage the inventory.
         * </summary>
         */
        public void ManageInventory()
        {
            if (!_inventoryShowed)
            {
                panInventory.SetActive(true);
            }
            else
            {
                panInventory.SetActive(false);
            }

            _inventoryShowed = !_inventoryShowed;
        }

        
        /**
         * <summary>
         * Create an item case in the inventory.
         * </summary>
         * <param name="item">The actual item.</param>
         */
        public void CreateItemInventory(Items item)
        {
            _itemCase = Instantiate(btnItem, panInventory.transform.GetChild(0).transform);
            _itemCase.GetComponent<Image>().sprite = item.ItemImage;
            _itemCase.GetComponent<Button>().onClick.AddListener(() => ManageItemInfo(item));       // Add a listener to the button.
        }

        
        /**
         * <summary>
         * Manage the information of the item.
         * </summary>
         * <param name="item">The actual item.</param>
         */
        private void ManageItemInfo(Items item)
        {
            imgItem.sprite = item.ItemImage;
            txtName.text = item.ItemName;
            txtPrice.text = item.ItemSellCost.ToString();
            btnDrop.GetComponent<Button>().onClick.AddListener(() => DropItem(item));
        }

        
        /**
         * <summary>
         * Drop the item from the player inventory.
         * </summary>
         * <param name="item">The actual item.</param>
         */
        private void DropItem(Items item)
        {
            _inventoryManager.RemoveItemFromInventory(item);
            Destroy(_itemCase);
        } 

        #endregion

    }
}
