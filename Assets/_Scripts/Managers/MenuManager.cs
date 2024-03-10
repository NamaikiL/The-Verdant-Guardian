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

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        private void Start()
        {
            _uiManager = UIManager.Instance;
            _audioManager = AudioManager.Instance;
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

        private IEnumerator DelayLoadGame()
        {
            _audioManager.BtnSFX.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            SceneManager.LoadScene("Map", LoadSceneMode.Single);
        }
        
        /**
         * <summary>
         * Load tuto scene.
         * </summary>
         */
        public void LoadTuto()
        {
            StartCoroutine(DelayLoadTuto());
        }

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

        private IEnumerator DelayLoadMenuInGame()
        {
            _audioManager.BtnSFX.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
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

        private IEnumerator DelayContinueGame()
        {
            yield return new WaitForSeconds(timeBeforeLoad);
            _uiManager.PauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion
    }
}