using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitWindowScreen : WindowScreenBase
{
    public override void OnCreate()
    {
        base.OnCreate();
        _screenSample = HSCL.ScreenSample.ExitAppScreen;
    }

    public void OnClick_ConfirmButton()
    {
        Application.Quit();
    }

    public void OnClick_CancelButton()
    {
        UIManager.Singeleton.DestroyTheFrontScreen();
    }
}
