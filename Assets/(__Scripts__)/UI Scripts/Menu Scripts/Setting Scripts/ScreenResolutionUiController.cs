using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolutionUiController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Dropdown dropdown;

    [SerializeField] Vector2Int[] resolutions = new Vector2Int[] { new Vector2Int(1920,1080),
                                                                   new Vector2Int(888,500),
                                                                   new Vector2Int(1280,720),
                                                                   new Vector2Int(1366,768),
                                                                   new Vector2Int(1600,900),
                                                                   new Vector2Int(1920,1080)};

    public void OnSelectResolution(int selectedCount)
    {
        int index = dropdown.value;
        Screen.SetResolution(resolutions[index].x, resolutions[index].y, Screen.fullScreenMode);
    }
}