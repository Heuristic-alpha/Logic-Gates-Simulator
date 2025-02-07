using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CameraSettingPanel : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] TMP_Text button_TEXT;
    [SerializeField] Slider cameraSizeSlider;
    CameraController cameraController;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Constant: ////////////////////////////////////////////////////////////
    private const string FULL_SCREEN_MODE_TEXT = "Full Screen Mode";
    private const string WINDOW_MODE_TEXT = "Window Mode";

    [SerializeField] float cameraZoomAmountChangeOnZoomButtonClicked = 0.5f;

    // C# Fields: //////////////////////////////////////////////////////////////
    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        Init_cameraSizeSlider();   
        if (cameraController == null) Debug.Log("camera controller is null");
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow) button_TEXT.text = FULL_SCREEN_MODE_TEXT;
        else if (Screen.fullScreenMode == FullScreenMode.Windowed) button_TEXT.text = WINDOW_MODE_TEXT;
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    private void Start()
    {
        cameraSizeSlider.value = cameraController.CameraCurrentSize;      
    }

    // C# Public Methods: //////////////////////////////////////////////////////
    public void ChangeScreenModeButton()
    {
        // change screen to FullScreenWindow mode
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            button_TEXT.text = FULL_SCREEN_MODE_TEXT;
           // lastWindowedSize = new Vector2Int()
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        // change screen to Window mode
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            button_TEXT.text = WINDOW_MODE_TEXT;
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void OnValueChange_CameraSizeSlider()
    {
        if (!Application.isPlaying) return;

        cameraController.SetCameraSizeExactly(cameraSizeSlider.value);
    }
    public void OnClickedOnZoomPlusButton()
    {
        cameraSizeSlider.value -= cameraZoomAmountChangeOnZoomButtonClicked;
    }
    public void OnClickedOnZoomMinusButton()
    {
        cameraSizeSlider.value += cameraZoomAmountChangeOnZoomButtonClicked;
    }

    // C# Private Methods: /////////////////////////////////////////////////////

    private void Init_cameraSizeSlider()
    {
        float length = cameraController.CameraMaxSize - cameraController.CameraMinSize;
        float t = cameraController.CameraCurrentSize / length;
        cameraSizeSlider.value = t;

        cameraSizeSlider.maxValue = cameraController.CameraMaxSize;
        cameraSizeSlider.minValue = cameraController.CameraMinSize;     
    }


} // end of class