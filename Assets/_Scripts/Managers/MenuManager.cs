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
        #endregion

        #region Custom Methods
        /**
         * <summary>
         * Load the game
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
        }

        /**
         * <summary>
         * Exit the game
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