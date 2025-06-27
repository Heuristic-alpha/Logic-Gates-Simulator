using HSCL;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveFileItem : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////  
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] TMP_Text _tmp_Text_fileName;
    [SerializeField] TMP_Text _tmp_Text_dateTime;
    [SerializeField] UnityEngine.UI.Image _baseColorImage;
    [SerializeField] UnityEngine.UI.Image _lineColorImage;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    public string Path;
    public string fileName;
    public string dateTime;
    public Color backColor;
    public Color lineColor;

    // Unity Main Events: //////////////////////////////////////////////////////
    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void Init(string path)
    {
        Path = path;
        if (Save_Load_System.Singeleton.GetSaveInfoAndSettingFromSaveFilePath(path, out fileName, out dateTime, out backColor, out lineColor))
        {         
            _tmp_Text_fileName.text = fileName;
            _tmp_Text_dateTime.text = dateTime;
            _baseColorImage.color = backColor;
            _lineColorImage.color = lineColor;
        }
        else
        {
            _tmp_Text_fileName.text = "Error";
            _tmp_Text_dateTime.text = "Error";
            _baseColorImage.color = Color.black;
            _lineColorImage.color = Color.black;
        }
        
    }

    public void OnClickButton()
    {
        SaveLoadInfoScreen infoScreen = UIManager.Singeleton.OpenAndReturnScreen<SaveLoadInfoScreen>(ScreenSample.SaveLoadInfoScreen);
        infoScreen.InitPanel(Path, fileName, dateTime, true);
    }
    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class