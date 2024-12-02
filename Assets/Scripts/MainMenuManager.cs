using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string MainSceneName = "MainScene";
    public void StartGame()
    {
        SceneManager.LoadScene(MainSceneName);
    }
}
