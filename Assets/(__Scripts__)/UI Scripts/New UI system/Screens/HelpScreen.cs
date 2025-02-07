using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : WindowScreenBase
{
    public override void OnCreate()
    {
        base.OnCreate();
        _screenSample = HSCL.ScreenSample.HelpScreen;
    }
}
