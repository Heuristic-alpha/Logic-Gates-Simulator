using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public string MainSceneName;
    public GameObject IntroSceneObject;
    private bool _mainSceneIsLoaded;
    private AsyncOperation _mainScene_operationHandle;


    private void Awake()
    {
        _mainSceneIsLoaded = false;
    }

    // //////////////////////////////////////////////////////////
    // Signal Receiver Events:
    public void OnSceneStart()
    {
        IntroSceneObject.SetActive(true);
        StartCoroutine(LoadMainSceneAsync());
    }
    public void OnIntroSceneEnd()
    {
        IntroSceneObject.SetActive(false);
        if (_mainSceneIsLoaded)
        {
            _mainScene_operationHandle.allowSceneActivation = true;
            Debug.Log("MainScene Start");
        }
        else
        {
            Debug.LogError("MainScene is not Loaded!");
        }
    }

    //////////////////////////////////////////////////////////
    private IEnumerator LoadMainSceneAsync()
    {
        _mainScene_operationHandle = SceneManager.LoadSceneAsync(MainSceneName);
        _mainScene_operationHandle.allowSceneActivation = false;
        while (_mainScene_operationHandle.progress < 0.9f)
        {
            yield return null;
            Debug.LogWarning($"Current Loading Time: {Time.timeSinceLevelLoad} --- Main Scene Load Progress: {_mainScene_operationHandle.progress}");
        }
        _mainSceneIsLoaded = true;
    }

}

