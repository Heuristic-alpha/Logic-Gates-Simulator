using HSCL;
using System.Collections;
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
    private Stack<ScreenBase> _screenStack;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        Singeleton = this;
        _screenStack = new Stack<ScreenBase>();       
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
    public void CreateScreen(HSCL.ScreenSample screenSample)
    {
        ScreenBase previousFrontScreen;
        if (_screenStack.Count > 0) 
        { 
            previousFrontScreen = _screenStack.Peek();
            previousFrontScreen.OnFocusLost();
        }

        GameObject screenPrefab = _screenSamplePrefabs[(int)screenSample];    
        GameObject newObject = Instantiate(screenPrefab, gameObject.transform, false);
        newObject.name = screenPrefab.name;
        _screenStack.Push(newObject.GetComponent<ScreenBase>());
        ScreenBase screen = _screenStack.Peek();
        screen.SetSortingLayerOfCanvas(_screenStack.Count - 1);
        screen.OnCreate();
        screen.OnFocus();
        screen.transform.SetAsLastSibling();
    }

    public T CreateAndReturnScreen<T>(ScreenSample screenSample) where T : ScreenBase
    {
        ScreenBase previousFrontScreen;
        if (_screenStack.Count > 0)
        {
            previousFrontScreen = _screenStack.Peek();
            previousFrontScreen.OnFocusLost();
        }

        GameObject screenPrefab = _screenSamplePrefabs[(int)screenSample];
        GameObject newObject = Instantiate(screenPrefab, gameObject.transform, false);
        newObject.name = screenPrefab.name;
        _screenStack.Push(newObject.GetComponent<ScreenBase>());
        ScreenBase screen = _screenStack.Peek();
        screen.SetSortingLayerOfCanvas(_screenStack.Count - 1);
        screen.OnCreate();
        screen.OnFocus();
        screen.transform.SetAsLastSibling();
        return (T)screen;
    }

    public void DestroyTheFrontScreen()
    {
        if (_screenStack.Count <= 0) return;
        // if the front screen is GamePlayScreen, we should not destroy it:
        if (_screenStack.Peek().GetScreenSample() == ScreenSample.GamePlayScreen) return;

        ScreenBase frontScreen = _screenStack.Peek();
        frontScreen.OnFocusLost();
        frontScreen.OnDestroyByUIManager();
        _screenStack.Pop();

        // if still ScreenBase Exist then:
        if (_screenStack.Count > 0)
        {
            ScreenBase newFrontScreen = _screenStack.Peek();
            newFrontScreen.OnFocus();
            newFrontScreen.transform.SetAsLastSibling();
        }
    }

    public bool TryGetScreen<T>(ScreenSample screenSample, out T returnScreen) where T : ScreenBase
    {
        foreach (ScreenBase screen in _screenStack)
        {
            if(screen.ScreenSample == screenSample)
            {
                returnScreen = (T) screen;
                return true;
            }
        }
        returnScreen = null;
        return false;
    }

    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class