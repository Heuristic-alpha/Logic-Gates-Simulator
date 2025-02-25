using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartSceneManager : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] private GameObject _introObject;
    [SerializeField] private GameObject _menuObject;
    [SerializeField] private GameObject _AppNameLogo;

    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] private TextMeshProUGUI _versionText;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] private string _mainSceneName;
    private AsyncOperation _mainSceneLoadOperation;
    private bool _mainSceneIsLoaded;
    private float _appNameDefaultPosY;
    private TMP_Text _appNameLogoText;
    private float _logoDefaultCharSpacing;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _mainSceneIsLoaded = false;
        _appNameLogoText = _AppNameLogo.GetComponent<TextMeshProUGUI>();
        _appNameDefaultPosY = _AppNameLogo.transform.localPosition.y;
        _logoDefaultCharSpacing = _appNameLogoText.characterSpacing;
        _versionText.text = $"Version <color=\"orange\">\'{Application.version}\'</color=\"orange\">";
    }

    private void Update()
    {
        //moving app logo:
        Vector3 newSize = _AppNameLogo.transform.localPosition;
        newSize.y = _appNameDefaultPosY + 50 * Mathf.Sin(Time.realtimeSinceStartup);
        _AppNameLogo.transform.localPosition = newSize;

        //change charcter spacing of logo:
        float newVal = _logoDefaultCharSpacing + Mathf.Sin(Time.realtimeSinceStartup);
        _appNameLogoText.characterSpacing = newVal;
    }
    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void OnStartSignalEvent()
    {
        _introObject.SetActive(true);
        _menuObject.SetActive(false);

        StartCoroutine(LoadMainScene());
    }
    public void OnEndSignalEvent()
        {
        _introObject.SetActive(false);
        _menuObject.SetActive(true);
        }

    public void OnClickStartButton()
        {
        if (!_mainSceneIsLoaded) return;
        _mainSceneLoadOperation.allowSceneActivation = true;
        }
    public void OnClickExitButton()
    {
        Application.Quit();
    }

    // C# Private Methods: /////////////////////////////////////////////////////

    private IEnumerator LoadMainScene()
    {
        _mainSceneLoadOperation = SceneManager.LoadSceneAsync(_mainSceneName, LoadSceneMode.Single);
        _mainSceneLoadOperation.allowSceneActivation = false;

        while (_mainSceneLoadOperation.isDone)
        {
            yield return null;

        }
        _mainSceneIsLoaded = true;
    }

} // end of class