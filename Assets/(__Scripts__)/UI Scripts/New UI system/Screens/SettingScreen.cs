using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingScreen : WindowScreenBase
{
    [SerializeField] GridUiPanelControler _gridUIPanelControler;

    public override void OnCreate()
    {
        base.OnCreate();
        _screenSample = HSCL.ScreenSample.SettingScreen;
    }
    public override void OnOpen()
    {
        base.OnOpen();
        _gridUIPanelControler.OnOpen();
    }
}
