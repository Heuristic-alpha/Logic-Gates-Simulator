using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerScreen : WindowScreenBase
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] private Slider _H_Slider;
    [SerializeField] private Slider _S_Slider;
    [SerializeField] private Slider _V_Slider;
    [SerializeField] private Image _previewImage;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    private event Action<Color> OnColorChange;

    // Unity Main Events: //////////////////////////////////////////////////////
    // Unity Other Events: /////////////////////////////////////////////////////

    private void OnDisable()
    {
        OnColorChange = null;
    }

    // C# Public Methods: //////////////////////////////////////////////////////
    public override void OnCreate()
    {
        base.OnCreate();
        _screenSample = HSCL.ScreenSample.ColorPickerScreen;
    }

    public void OnSlidersValueChanged()
    {
        _previewImage.color = Color.HSVToRGB(_H_Slider.value, _S_Slider.value, _V_Slider.value);
    }

    public void OnClickSetColorButton()
    {
        float H_Value = _H_Slider.value;
        float S_Value = _S_Slider.value;
        float V_Value = _V_Slider.value;

        OnColorChange?.Invoke(Color.HSVToRGB(H_Value, S_Value, V_Value));
        UIManager.Singeleton.CloseTheFrontScreen();
    }

    public void Init(Color importedColor, Action<Color> onColorChangedSubscribedMethod)
    {
        Color.RGBToHSV(importedColor, out float h, out float s, out float v);
        _H_Slider.value = h;
        _S_Slider.value = s;
        _V_Slider.value = v;
        OnColorChange = onColorChangedSubscribedMethod;
    }
    // C# Private Methods: /////////////////////////////////////////////////////  
}
