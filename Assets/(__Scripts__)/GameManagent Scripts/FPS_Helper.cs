using UnityEngine;

public static class FPS_Helper
{
   
    public static FPS_Value FPS_Value
    {
        get { return _FPS_Value; }
        set 
        { 
            _FPS_Value = value;
            Application.targetFrameRate = (int)_FPS_Value;
        }   
    }

    private static FPS_Value _FPS_Value;

    public static void Init_FPS()
    {

#if UNITY_ANDROID
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
#else 
        Application.targetFrameRate = -1;
#endif

    }


    public static void DeBugger()
    {
        Debug.Log($"FPS_Value:{FPS_Value} | TargetFrameRate:{Application.targetFrameRate} | QualitySettings.vSyncCount:{QualitySettings.vSyncCount}");
    }
}

public enum FPS_Value
{
    Thirty = 30,
    Sixty = 60,
    OneHundredTwenty = 120,
}