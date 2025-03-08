using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string MainSceneName = "MainScene";
    [SerializeField] string LoadingSceneName = "LoadingScene";
    public void StartGame()
    {
        StartCoroutine(OpenGameScene());
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
}
