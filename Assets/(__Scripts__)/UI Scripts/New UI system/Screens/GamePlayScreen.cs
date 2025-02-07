using HSCL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScreen : ScreenBase
{
    [SerializeField] GameObject FPS_counter_object;
    [SerializeField] GameObject Camera_Position_object;

    public override void OnCreate()
    {
        base.OnCreate();
        _screenSample = ScreenSample.GamePlayScreen;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Singeleton.CreateScreen(ScreenSample.ExitAppScreen);
        }
    }

    public void OnClickOpenMenuButton()
    {
        UIManager.Singeleton.CreateScreen(ScreenSample.MenuScreen);
    }

    public void Set_FPScounterobjectEnabled(bool enable) => FPS_counter_object.SetActive(enable);
    public void Set_CameraPositionobjectEnabled(bool enable) => Camera_Position_object.SetActive(enable);
    public GameObject Get_FPScounterobject() => FPS_counter_object;
    public GameObject Get_CameraPositionobject() => Camera_Position_object;
}
