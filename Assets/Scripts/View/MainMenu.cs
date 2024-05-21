using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.View
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string _zone1;
        [SerializeField] private string _zone2;

        public void PlayZone1()
        {
            SceneManager.LoadScene(_zone1);
        }

        public void PlayZone2()
        {
            SceneManager.LoadScene(_zone2);
        }

        public void QuitGame()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }
    }
}