using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers
{
    [RequireComponent(typeof(AudioSource))]

    public class MenuManager : MonoBehaviour
    {
        #region Variable

        [Header("Button manager")]
        [SerializeField] private float timeBeforeLoad = 0.5f;
        [SerializeField] private AudioSource btnSound;

        [Header("Scene Manager")]
        [SerializeField] private GameObject creditsScene;
        [SerializeField] private GameObject menuScene;

        //Component.
        private UIManager _uiManager;

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
            btnSound.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            SceneManager.LoadScene("Debug", LoadSceneMode.Single);

            //Game is not paused
            Time.timeScale = 1f;
            _uiManager.GameIsPaused = false;
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
            btnSound.Play();
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
            btnSound.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            creditsScene.SetActive(false);
            menuScene.SetActive(true);

            //Game is not paused
            Time.timeScale = 1f;
            _uiManager.GameIsPaused = false;
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
            _uiManager.GameIsPaused = false;
        }

        private IEnumerator DelayExitGame()
        {
            btnSound.Play();
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
            btnSound.Play();
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
            btnSound.Play();
            StartCoroutine(DelayContinueGame());

            //Game is not paused
            Time.timeScale = 1f;
            _uiManager.GameIsPaused = false;
        }

        private IEnumerator DelayContinueGame()
        {
            yield return new WaitForSeconds(timeBeforeLoad);
            _uiManager.PauseUI.SetActive(false);

            //Game is not paused
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion
    }
}