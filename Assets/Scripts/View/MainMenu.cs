using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.View
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneAsset _zone1;
        [SerializeField] private SceneAsset _zone2;

        public void PlayZone1()
        {
            SceneManager.LoadScene(_zone1.name);
        }

        public void PlayZone2()
        {
            SceneManager.LoadScene(_zone2.name);
        }

        public void QuitGame()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }
    }
}