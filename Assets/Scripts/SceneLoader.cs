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
        if(SceneManager.GetSceneByName(name) != null)
        {
            StartCoroutine(StartLoadingScene(name));
        }
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
}

