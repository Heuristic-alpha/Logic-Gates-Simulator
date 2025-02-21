using HSCL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridUiPanelControler : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    GameObject FPSPanelObject;
    GameObject cameraPosObject;

    // Unity Components: ///////////////////////////////////////////////////////
    BackGroundGridController _BackGroundGridController;
    ObjectToGridPosition _objectToGridPosition;
    [SerializeField] private Toggle _itemOnGridToggle;
    [SerializeField] private Toggle _showFPSToggle;
    [SerializeField] private Toggle _showCamPosToggle;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    // Unity Main Events: //////////////////////////////////////////////////////  

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////

    // most be called by SettingScreen
    public void OnOpen()
    {
        _objectToGridPosition = GameManager.Instance.GetObjectToGridPositionScript();
        if (_objectToGridPosition == null) Debug.LogWarning("Errooooooooooooooor");
        _BackGroundGridController = GameManager.Instance.GetBackGroundGridControllerScript();

        ScreenBase screen;
        if (!UIManager.Singeleton.TryGetScreen(HSCL.ScreenSample.GamePlayScreen, out screen))
        {
            Debug.LogError("cant find screen type of GamePlayScreen");
        }
        FPSPanelObject = (screen as GamePlayScreen).Get_FPScounterobject();
        cameraPosObject = (screen as GamePlayScreen).Get_CameraPositionobject();
        

        _itemOnGridToggle.isOn = _objectToGridPosition.postionToGridEnabled;
        _showFPSToggle.isOn = FPSPanelObject.activeSelf;
        _showCamPosToggle.isOn = cameraPosObject.activeSelf;
    }

    public void OnClick_changeBackGroundColorButton()
    {
        ColorPickerScreen screen = UIManager.Singeleton.OpenAndReturnScreen<ColorPickerScreen>(ScreenSample.ColorPickerScreen);
        screen.Init(_BackGroundGridController.Get_BackColor(), OnBackGroundColorChanged);
    }

    public void OnClick_changeLineColorButton()
    {
        ColorPickerScreen screen = UIManager.Singeleton.OpenAndReturnScreen<ColorPickerScreen>(ScreenSample.ColorPickerScreen);
        screen.Init(_BackGroundGridController.Get_LineColor(), OnLineColorChanged);
    }

    public void OnClick_ItemOnGridToggle()
    {
        _objectToGridPosition.postionToGridEnabled = _itemOnGridToggle.isOn;
    }
    public void OnClick_ShowFPSToggle()
    {       
        ScreenBase screen;
        if(!UIManager.Singeleton.TryGetScreen(HSCL.ScreenSample.GamePlayScreen, out screen))
        {
            Debug.LogError("cant find screen type of GamePlayScreen");
        }
        GameObject FPSPanelObject = (screen as GamePlayScreen).Get_FPScounterobject();
        FPSPanelObject.SetActive(_showFPSToggle.isOn);
    }
    public void OnClick_ShowCamPosToggle()
    {
        ScreenBase screen;
        if (!UIManager.Singeleton.TryGetScreen(HSCL.ScreenSample.GamePlayScreen, out screen))
        {
            Debug.LogError("cant find screen type of GamePlayScreen");
        }
        GameObject CameraPosObject = (screen as GamePlayScreen).Get_CameraPositionobject();
        CameraPosObject.SetActive(_showCamPosToggle.isOn);
    }

    // C# Private Methods: /////////////////////////////////////////////////////

    private void OnBackGroundColorChanged(Color color)
    {
        _BackGroundGridController.Set_BackColor(color);
    }

    private void OnLineColorChanged(Color color)
    {
        _BackGroundGridController.Set_LineColor(color);
    }

} // end of class