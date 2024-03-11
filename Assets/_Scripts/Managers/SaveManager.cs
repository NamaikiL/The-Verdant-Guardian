using System;
using System.Collections.Generic;
using System.IO;
using _Scripts.Gameplay.CharactersController.Player;
using _Scripts.Scriptables;
using _Scripts.UI.Inventory;
using UnityEngine;

namespace _Scripts.Managers
{

    /**
     * <summary>
     * Config save data.
     * </summary>
     */
    [Serializable]
    public class ConfigData
    {
        #region Variables

        [Header("Config Data")]
        [SerializeField] private bool hasSaved;

        #endregion

        #region Properties

        // Config Data Properties.
        public bool HasSaved
        {
            get => hasSaved;
            set => hasSaved = value;
        }

        #endregion
    }
   
   
    /**
     * <summary>
     * Player save data.
     * </summary>
     */
    [Serializable]
    public class PlayerData
    {
        #region Variables

        [Header("Player Coordinates")] 
        [SerializeField] private Vector3 playerPosition;
        [SerializeField] private Vector3 playerRotation;
        
        [Header("Player Level")]
        [SerializeField] private int playerLevel;
        [SerializeField] private int playerExperience;
        
        [Header("Player Skills")]
        [SerializeField] private int playerSkillPoints;
        [SerializeField] private int playerConstitutionPoints;
        [SerializeField] private int playerStrengthPoints;
        [SerializeField] private int playerVigorPoints;
        [SerializeField] private int playerDexterityPoints;
        [SerializeField] private int playerLuckPoints;

        #endregion

        #region Properties

        // Player Coordinates Properties.
        public Vector3 PlayerPosition => playerPosition;
        public Vector3 PlayerRotation => playerRotation;
        
        // Player Level Properties.
        public int PlayerLevel => playerLevel;
        public int PlayerExperience => playerExperience;
        
        // Player Skills Properties.
        public int PlayerSkillPoints => playerSkillPoints;
        public int PlayerConstitutionPoints => playerConstitutionPoints;
        public int PlayerStrengthPoints => playerStrengthPoints;
        public int PlayerVigorPoints => playerVigorPoints;
        public int PlayerDexterityPoints => playerDexterityPoints;
        public int PlayerLuckPoints => playerLuckPoints;

        #endregion
        
        #region Constructors Methods

        /**
         * <summary>
         * Constructor for the Player Data.
         * </summary>
         * <param name="playerPosition">The position of the player on the map.</param>
         * <param name="playerRotation">The rotation of the player on the map.</param>
         * <param name="playerLevel">The level of the player.</param>
         * <param name="playerExperience">The experience of the player.</param>
         * <param name="playerSkillPoints">The skill points of the player.</param>
         * <param name="playerConstitutionPoints">The constitution points of the player.</param>
         * <param name="playerStrengthPoints">The strength points of the player.</param>
         * <param name="playerVigorPoints">The vigor points of the player.</param>
         * <param name="playerDexterityPoints">The dexterity points of the player.</param>
         * <param name="playerLuckPoints">The luck points of the player.</param>
         */
        public PlayerData(Vector3 playerPosition, Vector3 playerRotation, int playerLevel, int playerExperience,
            int playerSkillPoints, int playerConstitutionPoints, int playerStrengthPoints,
            int playerVigorPoints, int playerDexterityPoints, int playerLuckPoints)
        {
            this.playerPosition = playerPosition;
            this.playerRotation = playerRotation;
            this.playerLevel = playerLevel;
            this.playerExperience = playerExperience;
            this.playerSkillPoints = playerSkillPoints;
            this.playerConstitutionPoints = playerConstitutionPoints;
            this.playerStrengthPoints = playerStrengthPoints;
            this.playerVigorPoints = playerVigorPoints;
            this.playerDexterityPoints = playerDexterityPoints;
            this.playerLuckPoints = playerLuckPoints;
        }

        #endregion
    }

    
    /**
     * <summary>
     * InventoryData save data.
     * </summary>
     */
    [Serializable]
    public class InventoryData
    {
        #region Variables

        [Header("Inventory Data")] 
        [SerializeField] private List<InventoryItem> inventoryItems;
        [SerializeField] private Items armorSO;
        [SerializeField] private Items weaponSO;

        #endregion

        #region Properties

        // Inventory Items Property
        public List<InventoryItem> InventoryItems => inventoryItems;
        public Items ArmorSO => armorSO;
        public Items WeaponSO => weaponSO;

        #endregion

        #region Constructor Methods

        /**
         * <summary>
         * Constructor for the Inventory Data.
         * </summary>
         * <param name="inventoryItems">The list of items in inventory.</param>
         * <param name="armorSO">The armor equipped in inventory.</param>
         * <param name="weaponSO">The weapon equipped in inventory.</param>
         */
        public InventoryData(List<InventoryItem> inventoryItems, Items armorSO, Items weaponSO)
        {
            this.inventoryItems = inventoryItems;
            this.armorSO = armorSO;
            this.weaponSO = weaponSO;
        }

        #endregion
    }
    
    
    /**
     * <summary>
     * Quests parameters data.
     * </summary>
     */
    [Serializable]
    public class QuestParametersData
    {
        #region Variables

        [Header("Quest Data")]
        [SerializeField] private bool isQuestActive;
        [SerializeField] private bool isQuestComplete;
        [SerializeField] private string questName;

        #endregion

        #region Properties

        // Quest Data Properties.
        public bool IsQuestActive
        {
            get => isQuestActive;
            set => isQuestActive = value;
        }

        public bool IsQuestComplete
        {
            get => isQuestComplete;
            set => isQuestComplete = value;
        }
        public string QuestName => questName;

        #endregion

        #region Constructor

        /**
         * <summary>
         * Constructor for the quest data.
         * </summary>
         * <param name="isQuestActive">Is the quest active.</param>
         * <param name="isQuestComplete">Is the quest complete.</param>
         * <param name="questName">Quest name.</param>
         */
        public QuestParametersData(bool isQuestActive, bool isQuestComplete, string questName)
        {
            this.isQuestActive = isQuestActive;
            this.isQuestComplete = isQuestComplete;
            this.questName = questName;
        }

        #endregion
    }
    
    
    /**
     * <summary>
     * Quests save data.
     * </summary>
     */
    [Serializable]
    public class QuestsData
    {
        #region Variables

        [Header("Quests Data")]
        [SerializeField] private List<QuestParametersData> questsParametersData = new List<QuestParametersData>();

        #endregion

        #region Properties

        public List<QuestParametersData> QuestParametersDatas => questsParametersData;

        #endregion
    }

    
    /**
     * <summary>
     * Objectives parameters data.
     * </summary>
     */
    [Serializable]
    public class ObjectiveParametersData
    {
        #region Variables

        [Header("Objective Data")]
        [SerializeField] private bool isObjectiveComplete;
        [SerializeField] private string objectiveName;
        [SerializeField] private int objectiveCollectNbCollected;
        [SerializeField] private int objectiveFightNbFighted;

        #endregion

        #region Properties

        // Objective Data Properties.
        public bool IsObjectiveComplete
        {
            get => isObjectiveComplete;
            set => isObjectiveComplete = value;
        }

        public string ObjectiveName => objectiveName;
        
        public int ObjectiveCollectNbCollected
        {
            get => objectiveCollectNbCollected;
            set => objectiveCollectNbCollected = value;
        }
        
        public int ObjectiveFightNbFighted
        {
            get => objectiveFightNbFighted;
            set => objectiveFightNbFighted = value;
        }

        #endregion

        #region Constructor

        /**
         * <summary>
         * Constructor for the quest data.
         * </summary>
         * <param name="isObjectiveComplete">Is the objective complete.</param>
         * <param name="objectiveName">Objective name.</param>
         * <param name="objectiveCollectNbCollected">Objective Collect Type State.</param>
         * <param name="objectiveFightNbFighted">Objective Fight Type State.</param>
         */
        public ObjectiveParametersData(bool isObjectiveComplete, string objectiveName, int objectiveCollectNbCollected, int objectiveFightNbFighted)
        {
            this.isObjectiveComplete = isObjectiveComplete;
            this.objectiveName = objectiveName;
            this.objectiveCollectNbCollected = objectiveCollectNbCollected;
            this.objectiveFightNbFighted = objectiveFightNbFighted;
        }

        #endregion
    }

    
    /**
     * <summary>
     * Objectives save data.
     * </summary>
     */
    [Serializable]
    public class ObjectivesData
    {
        #region Variables

        [Header("Objectives Data")] 
        [SerializeField] private List<ObjectiveParametersData> objectiveParametersData = new List<ObjectiveParametersData>();

        #endregion

        #region Properties

        public List<ObjectiveParametersData> ObjectiveParametersData => objectiveParametersData;

        #endregion
    }
    
    
    /**
     * <summary>
     * The script that manage the game save, by reading it's content to writing it.
     * </summary>
     */
    public class SaveManager : MonoBehaviour
    {
        #region Variables

        [Header("Equipment Values")] 
        [SerializeField] private EquipmentSlotUI armorSlotUI;
        [SerializeField] private EquipmentSlotUI weaponSlotUI;
        
        [Header("Data")]
        [SerializeField] private ConfigData configData = new ConfigData();
        [SerializeField] private PlayerData playerData;
        [SerializeField] private InventoryData inventoryData;
        [SerializeField] private QuestsData questsData;
        [SerializeField] private ObjectivesData objectivesData;
        
        // Saves Paths.
        private string _baseDataPath;
        private string _configDataPath;
        private string _playerDataPath;
        private string _inventoryDataPath;
        private string _questsDataPath;
        private string _objectivesDataPath;

        // Conditions.
        private bool _hasSaved;
        
        // Components.
        private InventoryManager _inventoryManager;

        // Singleton.
        private static SaveManager _instance;

        #endregion

        #region Properties

        // Singleton.
        public static SaveManager Instance => _instance;

        #endregion
        
        #region Built-In Methods

        /**
         * <summary>
         * Unity calls Awake when an enabled script instance is being loaded.
         * </summary>
         */
        void Awake()
        {
            // Singleton.
            if (_instance) Destroy(gameObject);
            _instance = this;

            // Initialize Methods.
            InitializePaths();
        }
        
        
        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start()
        {
            // Components.
            _inventoryManager = InventoryManager.Instance;
            
            if(File.Exists(_configDataPath))
            {
                LoadGameData();
                if(configData.HasSaved)
                    _hasSaved = true;
                else
                    _hasSaved = false;
            }
            else
                _hasSaved = false;
            
            if(!_hasSaved)
            {
                Debug.Log("Has not saved.");
                NewGameData();
            }
            else
            {
                Debug.Log("Has saved.");
                LoadGameData();
            }
        }

        #endregion

        #region Initialize Methods

        /**
         * <summary>
         * Initialize the paths.
         * </summary>
         */
        private void InitializePaths()
        {
            // Initialize the base path.
            _baseDataPath = Application.persistentDataPath;
            
            // Data Paths.
            _configDataPath = _baseDataPath + "/Config.json";
            _playerDataPath = _baseDataPath + "/PlayerData.json";
            _inventoryDataPath = _baseDataPath + "/InventoryData.json";
            _questsDataPath = _baseDataPath + "/QuestsData.json";
            _objectivesDataPath = _baseDataPath + "/ObjectivesData.json";
        }

        #endregion
        
        #region New Save Methods

        /**
         * Reset all the data of the save files.
         */
        private void NewGameData()
        {
            NewConfigData();
            NewPlayerData();
            NewInventoryData();
            NewQuestsData();
            NewObjectivesData();
        }


        /**
         * Reset the Config Data.
         */
        private void NewConfigData()
        {
            // Reset the values.
            configData.HasSaved = false;
            
            // Storing the values.
            string configDataValues = JsonUtility.ToJson(configData, true);
            File.WriteAllText(_configDataPath, configDataValues);
        }
        
        
        /**
         * <summary>
         * Reset the Player Data.
         * </summary>
         */
        private void NewPlayerData()
        {
            // Defining the default state of PlayerData.
            playerData = new PlayerData(
                new Vector3(236f, 41f, -222f), 
                new Vector3(0f, -49f, 0f),
                1, 
                0,
                0, 
                0, 
                0, 
                0, 
                0, 
                0
                );
            
            // Storing the values.
            string playerDataValue = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(_playerDataPath, playerDataValue);
        }


        /**
         * <summary>
         * Reset the Inventory Data.
         * </summary>
         */
        private void NewInventoryData()
        {
            // Defining the default state of Inventory Data.
            _inventoryManager.InventoryScriptable.InitializeInventory();
            inventoryData = new InventoryData(_inventoryManager.InventoryScriptable.CurrentInventoryItems, null, null);
            
            // Storing the values.
            string inventoryDataValue = JsonUtility.ToJson(inventoryData, true);
            File.WriteAllText(_inventoryDataPath, inventoryDataValue);
        }


        /**
         * <summary>
         * Reset the Quests Data.
         * </summary>
         */
        private void NewQuestsData()
        {
            // Getting all the existing quests.
            Quest[] listActualQuests = Resources.LoadAll<Quest>("Quests");

            // Defining the default state of Quests Data.
            foreach (Quest quest in listActualQuests)
            {
                // Resetting all the quests state.
                quest.IsComplete = false;
                quest.IsActive = false;
                
                questsData.QuestParametersDatas.Add(new QuestParametersData(quest.IsActive, quest.IsComplete, quest.QuestTitle));
            }
            
            // Storing the values.
            string questsDataValue = JsonUtility.ToJson(questsData, true);
            File.WriteAllText(_questsDataPath, questsDataValue);
        }


        /**
         * <summary>
         * Reset the Objectives Data.
         * </summary>
         */
        private void NewObjectivesData()
        {
            // Getting all the existing Objectives.
            Objectives[] listActualObjectives = Resources.LoadAll<Objectives>("Objectives");

            // Defining the default state of Objectives Data.
            foreach (Objectives objective in listActualObjectives)
            {
                // Resetting all the objectives state.
                objective.IsComplete = false;
                objective.NbCollected = 0;
                objective.NbFighted = 0;

                objectivesData.ObjectiveParametersData.Add(new ObjectiveParametersData(
                    objective.IsComplete,
                    objective.ObjectiveName, 
                    objective.NbCollected, 
                    objective.NbFighted
                    ));
            }
            
            // Storing the values.
            string objectivesDataValues = JsonUtility.ToJson(objectivesData, true);
            File.WriteAllText(_objectivesDataPath, objectivesDataValues);
        }

        #endregion

        #region Save Methods
        
        /**
         * Save all the data.
         */
        public void SaveGameData()
        {
            SaveConfigData();
            SavePlayerData();
            SaveInventoryData();
            SaveQuestsData();
            SaveObjectivesData();
        }
        
        
        /**
         * <summary>
         * Save the Config Data.
         * </summary>
         */
        private void SaveConfigData()
        {
            // Reset the values.
            configData.HasSaved = true;
            
            // Storing the values.
            string configDataValues = JsonUtility.ToJson(configData, true);
            File.WriteAllText(_configDataPath, configDataValues);
        }
        
        
        /**
         * <summary>
         * Save the Player Data.
         * </summary>
         */
        private void SavePlayerData()
        {
            // Defining PlayerData Coordinates && Defining PlayerData Stats.
            if(GameObject.FindWithTag("Player").TryGetComponent(out PlayerController playerController)
               && GameObject.FindWithTag("Player").TryGetComponent(out PlayerStats playerStats))
            {
                playerData = new PlayerData(
                    playerController.CurrentPlayerPosition,
                    playerController.CurrentPlayerRotation,
                    playerStats.CurrentPlayerLevel,
                    playerStats.CurrentPlayerExperience,
                    playerStats.CurrentPlayerSkillPoints,
                    playerStats.CurrentPlayerConstitutionPoints,
                    playerStats.CurrentPlayerStrengthPoints,
                    playerStats.CurrentPlayerVigorPoints,
                    playerStats.CurrentPlayerDexterityPoints,
                    playerStats.CurrentPlayerLuckPoints
                );
            }
            
            // Storing the values.
            string playerDataValue = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(_playerDataPath, playerDataValue);
        }


        /**
         * <summary>
         * Save the Inventory Data.
         * </summary>
         */
        private void SaveInventoryData()
        {
            // Defining the Inventory Data.
            inventoryData = new InventoryData(_inventoryManager.InventoryScriptable.CurrentInventoryItems, armorSlotUI.CurrentItem, weaponSlotUI.CurrentItem);
            
            // Storing the values.
            string inventoryDataValue = JsonUtility.ToJson(inventoryData, true);
            File.WriteAllText(_inventoryDataPath, inventoryDataValue);
        }


        /**
         * <summary>
         * Save the Quests Data.
         * </summary>
         */
        private void SaveQuestsData()
        {
            // Getting the Quests that exist.
            Quest[] listActualQuests = Resources.LoadAll<Quest>("Quests");

            // Defining the Quests Data.
            foreach (QuestParametersData quest in questsData.QuestParametersDatas)
            {
                int index = Array.FindIndex(listActualQuests, x => x.QuestTitle == quest.QuestName);
                quest.IsQuestActive = listActualQuests[index].IsActive;
                quest.IsQuestComplete = listActualQuests[index].IsComplete;
            }
            
            // Storing the values.
            string questsDataValues = JsonUtility.ToJson(questsData, true);
            File.WriteAllText(_questsDataPath, questsDataValues);
        }


        /**
         * <summary>
         * Save the Objectives Data.
         * </summary>
         */
        private void SaveObjectivesData()
        {
            // Getting the Quests that exist.
            Objectives[] listActualObjectives = Resources.LoadAll<Objectives>("Objectives");

            // Defining the Quests Data.
            foreach (ObjectiveParametersData objective in objectivesData.ObjectiveParametersData)
            {
                int index = Array.FindIndex(listActualObjectives, x => x.ObjectiveName == objective.ObjectiveName);
                objective.IsObjectiveComplete = listActualObjectives[index].IsComplete;
                objective.ObjectiveCollectNbCollected = listActualObjectives[index].NbCollected;
                objective.ObjectiveFightNbFighted = listActualObjectives[index].NbFighted;
            }
            
            // Storing the values.
            string objectivesDataValues = JsonUtility.ToJson(objectivesData, true);
            File.WriteAllText(_objectivesDataPath, objectivesDataValues);
        }

        #endregion

        #region Load Methods

        /**
         * <summary>
         * Load all the game data Json Files.
         * </summary>
         */
        private void LoadGameData()
        {
            LoadConfigData();
            LoadPlayerData();
            LoadInventoryData();
            LoadQuestsData();
            LoadObjectivesData();
        }


        /**
         * <summary>
         * Load the Config Data from the Json File.
         * </summary>
         */
        private void LoadConfigData()
        {
            string configDataJson = File.ReadAllText(_configDataPath);
            configData = JsonUtility.FromJson<ConfigData>(configDataJson);
        }
        
        
        /**
         * <summary>
         * Load the Player Data from the Json File.
         * </summary>
         */
        private void LoadPlayerData()
        {
            // Reading the Json Data.
            string playerDataJson = File.ReadAllText(_playerDataPath);
            playerData = JsonUtility.FromJson<PlayerData>(playerDataJson);
            
            // Apply the Player Coordinates Data.
            if (GameObject.FindWithTag("Player").TryGetComponent(out PlayerController playerController))
            {
                playerController.InitializePlayerCoordinates(
                    playerData.PlayerPosition, 
                    playerData.PlayerRotation
                    );
            }
            
            // Apply the Player Stats Data.
            if (GameObject.FindWithTag("Player").TryGetComponent(out PlayerStats playerStats))
            {
                // Apply Player Level Data.
                playerStats.InitializeLevel(
                    playerData.PlayerLevel, 
                    playerData.PlayerExperience
                    );
                
                // Apply Player Skills Data.
                playerStats.InitializePlayerSkills(
                    playerData.PlayerSkillPoints, 
                    playerData.PlayerConstitutionPoints,
                    playerData.PlayerStrengthPoints, 
                    playerData.PlayerVigorPoints, 
                    playerData.PlayerDexterityPoints,
                    playerData.PlayerLuckPoints
                    );
            }
        }


        /**
         * <summary>
         * Load the Inventory Data from the Json File.
         * </summary>
         */
        private void LoadInventoryData()
        {
            // Reading the Json Data.
            string inventoryDataJson = File.ReadAllText(_inventoryDataPath);
            inventoryData = JsonUtility.FromJson<InventoryData>(inventoryDataJson);
            
            // Apply the Inventory Data.
            _inventoryManager.InventoryScriptable.InitializeInventory(inventoryData.InventoryItems);
            armorSlotUI.InitializeEquippedItems(inventoryData.ArmorSO);
            weaponSlotUI.InitializeEquippedItems(inventoryData.WeaponSO);
        }


        /**
         * <summary>
         * Load the Quests Data from the Json File.
         * </summary>
         */
        private void LoadQuestsData()
        {
            // Getting the Quests that exist.
            Quest[] listActualQuests = Resources.LoadAll<Quest>("Quests");
            
            // Reading the Json Data.
            string questsDataJson = File.ReadAllText(_questsDataPath);
            questsData = JsonUtility.FromJson<QuestsData>(questsDataJson);

            // Apply the values.
            foreach (Quest quest in listActualQuests)
            {
                int index = questsData.QuestParametersDatas.FindIndex( x => x.QuestName == quest.QuestTitle);
                quest.IsActive = questsData.QuestParametersDatas[index].IsQuestActive;
                quest.IsComplete = questsData.QuestParametersDatas[index].IsQuestComplete;
            }
        }


        /**
         * <summary>
         * Load the Objectives Data from the Json File.
         * </summary>
         */
        private void LoadObjectivesData()
        {
            // Getting the Objectives that exist.
            Objectives[] listActualObjectives = Resources.LoadAll<Objectives>("Objectives");
            
            // Reading the Json Data.
            string objectivesDataJson = File.ReadAllText(_objectivesDataPath);
            objectivesData = JsonUtility.FromJson<ObjectivesData>(objectivesDataJson);

            // Apply the values.
            foreach (Objectives objective in listActualObjectives)
            {
                int index = objectivesData.ObjectiveParametersData.FindIndex( x => x.ObjectiveName == objective.ObjectiveName);
                objective.IsComplete = objectivesData.ObjectiveParametersData[index].IsObjectiveComplete;
                objective.NbCollected = objectivesData.ObjectiveParametersData[index].ObjectiveCollectNbCollected;
                objective.NbFighted = objectivesData.ObjectiveParametersData[index].ObjectiveFightNbFighted;
            }
        }

        #endregion
    }
}
