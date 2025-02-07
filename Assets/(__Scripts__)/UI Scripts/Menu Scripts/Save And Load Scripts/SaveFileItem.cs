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

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    public string Path;
    public string fileName;
    public string dateTime;

    // Unity Main Events: //////////////////////////////////////////////////////
    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void Init(string path)
    {
        Path = path;
        if (Save_Load_System.Singeleton.GetSaveInfoFromSaveFilePath(path, out fileName, out dateTime))
        {         
            _tmp_Text_fileName.text = fileName;
            _tmp_Text_dateTime.text = dateTime;
        }
        else
        {
            _tmp_Text_fileName.text = "Error";
            _tmp_Text_dateTime.text = "Error";
        }
        
    }

    public void OnClickButton()
    {
        SaveLoadInfoScreen infoScreen = UIManager.Singeleton.CreateAndReturnScreen<SaveLoadInfoScreen>(ScreenSample.SaveLoadInfoScreen);
        infoScreen.InitPanel(Path, fileName, dateTime, true);
    }
    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class