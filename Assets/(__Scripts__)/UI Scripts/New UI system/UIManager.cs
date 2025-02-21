using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    public static UIManager Singeleton { get; private set; }

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField, Header("screenSamplePrefabs")]
    public List<GameObject> _screenSamplePrefabs;
    private Stack<HSCL.ScreenBase> _screenStack;

    [Tooltip("if cash mode is enabled all screen will be cashed and not be destroyed, they just disabled and if needed then enabled")]
    [SerializeField] private bool _cashMode;
    private List<GameObject> _cashedScreenObjects; // cashedScreenObjects stored disabled screenGameObjects


    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        Singeleton = this;
        _screenStack = new Stack<HSCL.ScreenBase>();
        if (_cashMode)
        {
            _cashedScreenObjects = new List<GameObject>();
            CreateAllScreenInCashMode();
        }
    }

    private void Update()
    {
        _screenStack.Peek().OnUpdate();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    private void OnDestroy()
    {
        Singeleton = null;
    }

    // C# Public Methods: //////////////////////////////////////////////////////
    public void OpenScreen(HSCL.ScreenSample screenSample)
    {
        if (_cashMode)
        {
            HSCL.ScreenBase screen = GetScreenInCashMode<HSCL.ScreenBase>(screenSample);
            screen.gameObject.SetActive(true);
            _screenStack.Push(screen);
            screen.SetSortingLayerOfCanvas(_screenStack.Count - 1);
            screen.OnOpen();
            screen.OnFocus();
            screen.transform.SetAsLastSibling();
        }
        else
        {
            HSCL.ScreenBase previousFrontScreen;
            if (_screenStack.Count > 0)
            {
                previousFrontScreen = _screenStack.Peek();
                previousFrontScreen.OnFocusLost();
            }

            GameObject screenPrefab = _screenSamplePrefabs[(int)screenSample];
            GameObject newObject = Instantiate(screenPrefab, gameObject.transform, false);
            newObject.name = screenPrefab.name;
            _screenStack.Push(newObject.GetComponent<HSCL.ScreenBase>());
            HSCL.ScreenBase screen = _screenStack.Peek();
            screen.SetSortingLayerOfCanvas(_screenStack.Count - 1);
            screen.OnCreate();
            screen.OnOpen();
            screen.OnFocus();
            screen.transform.SetAsLastSibling();
        }
    }

    public T OpenAndReturnScreen<T>(HSCL.ScreenSample screenSample) where T : HSCL.ScreenBase
    {
        if (_cashMode)
        {
            HSCL.ScreenBase screen = GetScreenInCashMode<HSCL.ScreenBase>(screenSample);
            screen.gameObject.SetActive(true);
            _screenStack.Push(screen);
            screen.SetSortingLayerOfCanvas(_screenStack.Count - 1);
            screen.OnOpen();
            screen.OnFocus();
            screen.transform.SetAsLastSibling();
            return (T)screen;
        }
        else
        {
            HSCL.ScreenBase previousFrontScreen;
            if (_screenStack.Count > 0)
            {
                previousFrontScreen = _screenStack.Peek();
                previousFrontScreen.OnFocusLost();
            }

            GameObject screenPrefab = _screenSamplePrefabs[(int)screenSample];
            GameObject newObject = Instantiate(screenPrefab, gameObject.transform, false);
            newObject.name = screenPrefab.name;
            _screenStack.Push(newObject.GetComponent<HSCL.ScreenBase>());
            HSCL.ScreenBase screen = _screenStack.Peek();
            screen.SetSortingLayerOfCanvas(_screenStack.Count - 1);
            screen.OnCreate();
            screen.OnOpen();
            screen.OnFocus();
            screen.transform.SetAsLastSibling();
            return (T)screen;
        }
    }

    public void CloseTheFrontScreen()
    {
        if (_screenStack.Count <= 0) return;
        // if the front screen is GamePlayScreen, we should not destroy or deActive it:
        if (_screenStack.Peek().GetScreenSample() == HSCL.ScreenSample.GamePlayScreen) return;

        HSCL.ScreenBase frontScreen = _screenStack.Peek();
        frontScreen.OnFocusLost();
        frontScreen.OnClosedByUIManager(_cashMode);
        _screenStack.Pop();
        if (_cashMode) _cashedScreenObjects.Add(frontScreen.gameObject);

        // if still ScreenBase Exist then:
        if (_screenStack.Count > 0)
        {
            HSCL.ScreenBase newFrontScreen = _screenStack.Peek();
            newFrontScreen.OnFocus();
            newFrontScreen.transform.SetAsLastSibling();
        }

    }

    public bool TryGetScreen<T>(HSCL.ScreenSample screenSample, out T returnScreen) where T : HSCL.ScreenBase
    {
        foreach (HSCL.ScreenBase screen in _screenStack)
        {
            if (screen.ScreenSample == screenSample)
            {
                returnScreen = (T)screen;
                return true;
            }
        }
        returnScreen = null;
        return false;
    }

    private void CreateAllScreenInCashMode()
    {
        foreach (GameObject prefab in _screenSamplePrefabs)
        {
            if (prefab == null) continue;
            GameObject newObject = Instantiate(prefab, gameObject.transform, false);
            _cashedScreenObjects.Add(newObject);
            newObject.name = prefab.name;
            newObject.SetActive(false);
            HSCL.ScreenBase screenBase = newObject.GetComponent<HSCL.ScreenBase>();
            screenBase.OnCreate();
        }
    }

    private T GetScreenInCashMode<T>(HSCL.ScreenSample screenSample) where T : HSCL.ScreenBase
    {
        foreach (GameObject go in _cashedScreenObjects)
        {
            HSCL.ScreenBase screenBase = go.GetComponent<HSCL.ScreenBase>();
            if (screenBase.ScreenSample == screenSample)
            {
                _cashedScreenObjects.Remove(go);
                return (T)screenBase;
            }
            else continue;
        }
        Debug.LogError($"Cant find any Screen of Type {screenSample} in cashedScreenObjects.");
        return null;
    }

    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class