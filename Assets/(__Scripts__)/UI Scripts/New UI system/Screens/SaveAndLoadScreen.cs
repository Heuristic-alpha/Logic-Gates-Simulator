using HSCL;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoadPanelScreen : WindowScreenBase
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject _fileContentHolder;
    [SerializeField] GameObject _saveFileItemPrefab;

    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] Toggle _autoSaveTempToggle;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    // Unity Main Events: //////////////////////////////////////////////////////
    // Unity Other Events: /////////////////////////////////////////////////////  
    // C# Public Methods: //////////////////////////////////////////////////////
    public override void OnCreate()
    {
        base.OnCreate();
        _screenSample = HSCL.ScreenSample.SaveLoadScreen;
        
    }
    public override void OnOpen()
    {
        _autoSaveTempToggle.isOn = Save_Load_System.Singeleton.TempSave;
        BrowseInSaveFolderAndGenerateSaveFileItems();
    }

    public void OnAutoSaveTempToggleClicked()
    {
        Save_Load_System.Singeleton.TempSave = _autoSaveTempToggle.isOn;
    }
    public void OnClickedOn_CreateNewSave_Button()
    {
        SaveLoadInfoScreen screen = UIManager.Singeleton.OpenAndReturnScreen<SaveLoadInfoScreen>(ScreenSample.SaveLoadInfoScreen);
        screen.InitPanel(string.Empty, string.Empty, string.Empty, false);
    }
    public void RefreshFileItems()
    {
        BrowseInSaveFolderAndGenerateSaveFileItems();
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void BrowseInSaveFolderAndGenerateSaveFileItems()
    {
        DeleteOldSaveFileItems();
        string[] savePathes = Save_Load_System.Singeleton.GetPathToExistSaveFiles();
        foreach (string path in savePathes)
        {
            GameObject spawnedObject = Instantiate(_saveFileItemPrefab, Vector3.zero, Quaternion.identity);
            spawnedObject.transform.SetParent(_fileContentHolder.transform);
            spawnedObject.transform.localScale = Vector3.one;
            spawnedObject.GetComponent<SaveFileItem>().Init(path);
        }
    }

    private void DeleteOldSaveFileItems()
    {
        int childCount = _fileContentHolder.transform.childCount;
        GameObject[] children = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            children[i] = _fileContentHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < children.Length; i++)
        {
            DestroyImmediate(children[i]);
        }
    }

} // end of class
