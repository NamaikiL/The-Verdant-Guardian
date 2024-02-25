using System.IO;
using UnityEngine;

namespace _Scripts.Managers
{
    /**
     * <summary>
     * Custom Data Class for example.
     * </summary>
     */
    class InfoTestJson
    { 
        public string donneeSensible;
        public string positionsDeLaCible;
    }
    
    
    /**
     * <summary>
     * The script that manage the game save, by reading it's content to saving it.
     * </summary>
     */
    public class SaveManager : MonoBehaviour
    {
        #region Variables

        [Header("Test Data")]
        [SerializeField] private InfoTestJson _info = new InfoTestJson();

        #endregion

        #region Save Management

        /**
         * <summary>
         * Example of saving file.
         * </summary>
         */
        void SaveToJson()
        {
            _info.donneeSensible = "sgzgzgezgz";
            _info.positionsDeLaCible = "sgzgzgezgz";
            string valueToSave = JsonUtility.ToJson(_info);
            File.WriteAllText(Application.persistentDataPath + "/infos.json", valueToSave);
        }

        
        /**
         * <summary>
         * Example of reading file.
         * </summary>
         */
        void ReadFromJson()
        {
            string path = Application.persistentDataPath + "/infos.json";
            string json = File.ReadAllText(path);
            Debug.Log(json);

            _info = JsonUtility.FromJson<InfoTestJson>(json);
        }

        #endregion
    }
}
