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

        //Condition.
        private bool _gameIsPaused;

        #endregion

        #region Custom Methods

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
            Time.timeScale = 1f;
            _gameIsPaused = false;
        }
        
        /**
         * <summary>
         * Load Credit scene.
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
         * Load Credit scene.
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
            Time.timeScale = 1f;
            _gameIsPaused = false;
        }

        /**
         * <summary>
         * Exit the game.
         * </summary>
         */
        public void ExitGame()
        {
            StartCoroutine (DelayExitGame());
        }

        private IEnumerator DelayExitGame()
        {
            btnSound.Play();
            yield return new WaitForSeconds(timeBeforeLoad);
            Debug.Log("Exit Game");
            Application.Quit();
        }

        #endregion
    }
}