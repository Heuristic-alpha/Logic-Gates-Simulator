using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HSCL
{
    /// <summary>
    /// basic ScreenBase class that other type of screens will be inheritance from this class
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class ScreenBase : MonoBehaviour
    {
        protected Canvas _canvas;
        protected GraphicRaycaster _graphicRaycaster;
        protected ScreenSample _screenSample;
        protected bool _isFocused;

        public ScreenSample ScreenSample { get { return _screenSample; } }
      
        protected void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.worldCamera = Camera.main;
            _graphicRaycaster = GetComponent<GraphicRaycaster>();
            // _screenSample most be Override on every drived class:
            _screenSample = ScreenSample.None;
            _isFocused = false;
        }

        public ScreenSample GetScreenSample() { return _screenSample; }

        public virtual void OnCreate()
        {
            Debug.Log($"ScreenBase of type {gameObject.name} has Created.");
        }
        public virtual void OnDestroyByUIManager()
        {
            Destroy(gameObject);
            Debug.Log($"ScreenBase of type {gameObject.name} has Destroyed.");

        }
        public virtual void OnFocus()
        {
            _isFocused = true;
            _graphicRaycaster.enabled = _isFocused;
            Debug.Log($"ScreenBase of type {gameObject.name} has Focused.");          
        }
        public virtual void OnFocusLost()
        {
            _isFocused = false;
            _graphicRaycaster.enabled = _isFocused;
            Debug.Log($"ScreenBase of type {gameObject.name} has Focus Losed.");
        }
        public virtual void OnUpdate()
        {

        }     
        
        public void SetSortingLayerOfCanvas(int index)
        {
            _canvas.sortingOrder = index;
        }

    }

    public enum ScreenSample
    {
        None = 0,
        WindowScreenBase,
        GamePlayScreen,
        MenuScreen,
        SettingScreen,
        SaveLoadScreen,
        ColorPickerScreen,
        ExitAppScreen,
        ContextMenuScreen,
        SaveLoadInfoScreen,
        HelpScreen,
    }
}