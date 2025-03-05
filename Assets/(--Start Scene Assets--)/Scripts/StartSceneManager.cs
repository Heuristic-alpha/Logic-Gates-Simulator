using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class StartSceneManager : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] private GameObject _introObject;
    [SerializeField] private GameObject _menuObject;
    [SerializeField] private GameObject _AppNameLogo;
    [SerializeField] private GameObject _menuBackGroundImageObject;

    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] private TextMeshProUGUI _versionText;
    UnityEngine.UI.Image _menuBackGroundImage;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] Color _firstColor;
    [SerializeField] Color _secondColor;
    [SerializeField, Range(1, 10)] int colorChangingSpeed;
    [SerializeField] string _mainSceneName;
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
        _versionText.text = $"Version <b><color=\"orange\">\'{Application.version}\'</color=\"orange\"></b>";
        _menuBackGroundImage = _menuBackGroundImageObject.GetComponent<UnityEngine.UI.Image>();
    }

    private void Update()
    {
        //moving app logo:
        Vector3 newSize = _AppNameLogo.transform.localPosition;
        newSize.y = _appNameDefaultPosY + 50 * Mathf.Sin(Time.realtimeSinceStartup);
        _AppNameLogo.transform.localPosition = newSize;

        //change charcter spacing of logo:
        float newVal = _logoDefaultCharSpacing + 6 * Mathf.Sin(Time.realtimeSinceStartup);
        _appNameLogoText.characterSpacing = newVal;

        ChangeMenuBackGroundImage();
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

        while (_mainSceneLoadOperation.progress < 0.9f)
        {
            yield return null;      
           // Debug.LogWarning($"Current Loading Time: {Time.timeSinceLevelLoad} --- Main Scene Load Progress: {_mainSceneLoadOperation.progress}");
        }
        _mainSceneIsLoaded = true;
    }

    float colorTime;
    int colorIncreamentDir = 1;
    private void ChangeMenuBackGroundImage()
    {
        _menuBackGroundImage.color = Color.Lerp(_firstColor, _secondColor, colorTime);

        colorTime += Time.deltaTime * colorIncreamentDir * colorChangingSpeed / 10;
        if (colorTime > 1)
        {
            colorTime = 1;
            colorIncreamentDir = -1;
        }
        else if (colorTime < 0)
        {
            colorTime = 0;
            colorIncreamentDir = 1;
        }
    }


} // end of class
