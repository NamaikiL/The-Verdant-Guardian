using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers
{
    /**
     * <summary>
     * Manage the menu in general.
     * </summary>
     */
    public class MenuManager : MonoBehaviour
    {
        #region Variable

        [Header("Button manager")]
        [SerializeField] private float timeBeforeLoad = 0.5f;

        [Header("Scene Manager")]
        [SerializeField] private GameObject creditsScene;
        [SerializeField] private GameObject menuScene;
        [SerializeField] private GameObject tutoScene;

        //Components.
        private UIManager _uiManager;
        private AudioManager _audioManager;
        private SaveManager _saveManager;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start()
        {
            //Components.
            _uiManager = UIManager.Instance;
            _audioManager = AudioManager.Instance;
            _saveManager = SaveManager.Instance;
        }
        
        #endregion

        #region Menu Methods

        /**
         * <summary>
         * Load the game.
         * </summary>
         */
        public void LoadGame()
        {
            StartCoroutine (DelayLoadGame());
        }

        
        /**
         * <summary>
         * Game Loading Delay.
         * </summary>
         */
        private IEnumerator DelayLoadGame()
        {
            _audioManager.BtnSFX.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            SceneManager.LoadScene("Map", LoadSceneMode.Single);
        }
        
        
        /**
         * <summary>
         * Load tutorial scene.
         * </summary>
         */
        public void LoadTuto()
        {
            StartCoroutine(DelayLoadTuto());
        }

        
        /**
         * <summary>
         * Tutorial loading delay.
         * </summary>
         */
        private IEnumerator DelayLoadTuto()
        {
            _audioManager.BtnSFX.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            tutoScene.SetActive(true);
            menuScene.SetActive(false);
        }
        
        
        /**
         * <summary>
         * Load credit scene.
         * </summary>
         */
        public void LoadCredits()
        {
            StartCoroutine(DelayLoadCredits());
        }

        
        /**
         * <summary>
         * Credits loading delay.
         * </summary>
         */
        private IEnumerator DelayLoadCredits()
        {
            _audioManager.BtnSFX.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            creditsScene.SetActive(true);
            menuScene.SetActive(false);
        }

        
        /**
         * <summary>
         * Load menu scene.
         * </summary>
         */
        public void LoadMenu()
        {
            StartCoroutine(DelayLoadMenu());
        }

        
        /**
         * <summary>
         * Menu loading delay.
         * </summary>
         */
        private IEnumerator DelayLoadMenu()
        {
            _audioManager.BtnSFX.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            creditsScene.SetActive(false);
            menuScene.SetActive(true);
        }

        
        /**
         * <summary>
         * Exit the game.
         * </summary>
         */
        public void ExitGame()
        {
            StartCoroutine (DelayExitGame());

            //Game is not paused
            Time.timeScale = 1f;
        }

        
        /**
         * <summary>
         * Delay the exit game.
         * </summary>
         */
        private IEnumerator DelayExitGame()
        {
            _audioManager.BtnSFX.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            Application.Quit();
        }

        #endregion

        #region Menu In Game Methods

        /**
         * <summary>
         * Load menu scene.
         * </summary>
         */
        public void LoadMenuInGame()
        {
            StartCoroutine(DelayLoadMenuInGame());

            //Game is not paused
            Time.timeScale = 1f;
            _uiManager.GameIsPaused = false;
        }

        
        /**
         * <summary>
         * In game menu loading with delay.
         * </summary>
         */
        private IEnumerator DelayLoadMenuInGame()
        {
            _audioManager.BtnSFX.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        
        /**
         * <summary>
         * Save game data.
         * </summary>
         */
        public void SaveGame()
        {
            StartCoroutine(DelaySaveGame());

            //Game is not paused
            Time.timeScale = 1f;
            _uiManager.GameIsPaused = false;
        }

        
        /**
         * <summary>
         * In game saving with delay.
         * </summary>
         */
        private IEnumerator DelaySaveGame()
        {
            _audioManager.BtnSFX.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            _saveManager.SaveGameData();
        }

        
        /**
         * <summary>
         * Continue to play.
         * </summary>
         */
        public void ContinueGame()
        {
            _audioManager.BtnSFX.Play();
            StartCoroutine(DelayContinueGame());

            //Game is not paused
            Time.timeScale = 1f;
            _uiManager.GameIsPaused = false;
        }

        
        /**
         * <summary>
         * Continue game loading with delay.
         * </summary>
         */
        private IEnumerator DelayContinueGame()
        {
            yield return new WaitForSeconds(timeBeforeLoad);
            _uiManager.PauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        #endregion
    }
}