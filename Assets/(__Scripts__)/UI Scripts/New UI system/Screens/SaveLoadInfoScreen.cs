using HSCL;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SaveLoadInfoScreen : WindowScreenBase
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject _lastEditDateTimeGameObject;

    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private TMP_Text _panelNameTXT;
    [SerializeField] private TMP_InputField _saveNameTXT;
    [SerializeField] private TMP_Text _dateTimeTXT;
    [SerializeField] private TMP_FontAsset _EN_font;
    [SerializeField] private TMP_FontAsset _FA_font;

    private SaveAndLoadPanelScreen _saveLoadScreen;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    private const string SAVE_INFO_EN = "Save info";
    private const string NEW_SAVE_EN = "New save";
    private const string SAVE_INFO_FA = "اطلاعات ذخیره شده";
    private const string NEW_SAVE_FA = "ایجاد ذخیره جدید";
    private string _newSaveName = "";
    private char[] _RANDOM_CHARS = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'm', 'l', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    private char[] _forbiden_Chars = { '<', '/', '>', '!', '\\', '\"', '\'' };
    private bool _isInInfoMode;

    private string _saveFilePath;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Start()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    // Unity Other Events: /////////////////////////////////////////////////////

    // C# Public Methods: //////////////////////////////////////////////////////
    public override void OnCreate()
    {
        base.OnCreate();
        _screenSample = HSCL.ScreenSample.SaveLoadInfoScreen;

    }

    public override void OnOpen()
    {
        base.OnOpen();
        if (!UIManager.Singeleton.TryGetScreen<SaveAndLoadPanelScreen>(ScreenSample.SaveLoadScreen, out _saveLoadScreen))
        {
            Debug.LogError("SaveLoadInfoScreen : SaveAndLoadPanelScreen is not found! ");
        }
    }

    public void InitPanel(string saveFilePath, string fileName, string dateTime, bool openInInfoMode)
    {
        _saveFilePath = saveFilePath;
        _isInInfoMode = openInInfoMode;
        if (openInInfoMode) // save info mode
        {
            _saveButton.interactable = true;
            _loadButton.interactable = true;
            _deleteButton.interactable = true;
            _panelNameTXT.text = SAVE_INFO_EN;
            _saveNameTXT.text = fileName;
            _lastEditDateTimeGameObject.SetActive(true);
            _dateTimeTXT.text = dateTime;
        }
        else   // new save mode
        {
            _saveButton.interactable = true;
            _loadButton.interactable = false;
            _deleteButton.interactable = false;
            _panelNameTXT.text = NEW_SAVE_EN;
            _lastEditDateTimeGameObject.SetActive(false);
            _saveNameTXT.text = _newSaveName;
        }
        // update window header:
        OnLocaleChanged(LocalizationSettings.SelectedLocale);
    }
    public override void OnClickCloseWindowButton()
    {
        base.OnClickCloseWindowButton();
    }

    public void OnPressSaveButton()
    {
        if (_isInInfoMode)
        {
            Save_Load_System.Singeleton.CreateSaveFileFromPath(_saveFilePath, _saveNameTXT.text);
        }
        else
        {
            Save_Load_System.Singeleton.CreateSaveFileFromName(GenerateRandomString(), _saveNameTXT.text);
        }
        OnClickCloseWindowButton();
    }
    public void OnPressLoadButton()
    {
        Save_Load_System.Singeleton.LoadSaveFileFromPath(_saveFilePath);
        OnClickCloseWindowButton();

        //close Save & Load Screen:
        UIManager.Singeleton.CloseTheFrontScreen();
    }
    public void OnPressDeleteButton()
    {
        Save_Load_System.Singeleton.DeleteSaveFileFromPath(_saveFilePath);
        OnClickCloseWindowButton();
    }

    // validate input field chars:
    public void OnSaveNameTxtChange(string noThing)
    {
        string content = _saveNameTXT.text;
        int length = content.Length;
        if (length < 1) return;

        char lastChar = content[length - 1];
        foreach (char forbidChar in _forbiden_Chars)
        {
            if (forbidChar == lastChar)
            {
                _saveNameTXT.text = content.Substring(0, length - 1);
                break;
            }
        }

    }

    // C# Private Methods: /////////////////////////////////////////////////////

    private string GenerateRandomString()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.ToBinary());
        int stringLength = 9;
        StringBuilder random_sb = new StringBuilder();
        for (int i = 0; i < stringLength; i++)
        {
            random_sb.Append(_RANDOM_CHARS[UnityEngine.Random.Range(0, _RANDOM_CHARS.Length)]);
        }
        return $"({_saveNameTXT.text})_{random_sb.ToString()}{DateTime.Now.Ticks.ToString()}";
    }

    private void OnLocaleChanged(Locale locale)
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (_isInInfoMode)
        {
            if (locale == locales[0]) // en
            {
                _panelNameTXT.text = SAVE_INFO_EN;
                _panelNameTXT.font = _EN_font;
            }
            else if (locale == locales[1]) // fa
            {
                _panelNameTXT.text = SAVE_INFO_FA;
                _panelNameTXT.font = _FA_font;
            }
        }
        else // new save mode
        {
            if (locale == locales[0]) // en
            {
                _panelNameTXT.text = NEW_SAVE_EN;
                _panelNameTXT.font = _EN_font;
            }
            else if (locale == locales[1]) // fa
            {
                _panelNameTXT.text = NEW_SAVE_FA;
                _panelNameTXT.font = _FA_font;
            }
        }
    }
}
