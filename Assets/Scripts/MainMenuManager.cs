using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string MainSceneName = "MainScene";
    [SerializeField] string LoadingSceneName = "LoadingScene";
    [SerializeField] string MenuSceneName = "MainMenu";
    public void StartGame()
    {
        StartCoroutine(OpenGameScene());
        DontDestroyOnLoad(gameObject);
    }

    public void QuitToMenu()
    {
        Debug.Log("Menuing...");
        StartCoroutine(OpenMenuScene());
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator OpenGameScene()
    {
        yield return new WaitForSeconds(1f);
        var asyncLoading = SceneManager.LoadSceneAsync(LoadingSceneName);
        asyncLoading.completed += AsyncLoading_completed;
    }

    private void AsyncLoading_completed(AsyncOperation obj)
    {
        obj.completed -= AsyncLoading_completed;
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(MainSceneName);
        Destroy(gameObject);
    }

    public IEnumerator OpenMenuScene()
    {
        yield return new WaitForSeconds(1f);
        var asyncLoading = SceneManager.LoadSceneAsync(LoadingSceneName);
        yield return new WaitUntil(() => asyncLoading.progress >= 0.9f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(MenuSceneName);
        Destroy(gameObject);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying needs to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("Quitting...");
#else
        Application.Quit();
#endif
    }
}
