using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void LoadScene(string name)
    {
        if(name == null || name.Length == 0)
        {
            Debug.LogWarning("No name provided when loading scene, loading next instead");
            LoadNext();
        }

        if(SceneManager.GetSceneByName(name) != null)
        {
            StartCoroutine(StartLoadingScene(name));
        }
    }

    public void LoadScene(int index)
    {
        if(SceneManager.GetSceneByBuildIndex(index) != null)
        {
            StartCoroutine(StartLoadingScene(index));
        }
    }

    public void LoadNext()
    {
        LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    private IEnumerator StartLoadingScene(string name)
    {
        yield return null;
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(name);
        while (!sceneLoading.isDone)
        {
            Debug.Log($"Scene loading in progress, current progress: {sceneLoading.progress}");
            yield return null;
        }
        yield return null;
    }

    private IEnumerator StartLoadingScene(int index)
    {
        yield return null;
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(index);
        while (sceneLoading != null && !sceneLoading.isDone)
        {
            Debug.Log($"Scene loading in progress, current progress: {sceneLoading.progress}");
            yield return null;
        }

        Debug.Log("Loaded scene!");
        yield return null;
    }

    public static void OnStartButton()
    {
        Instance.LoadNext();
    }

    public static void OnQuitButton()
    {
        Application.Quit();
    }
}

