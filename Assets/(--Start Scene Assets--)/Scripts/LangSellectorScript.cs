using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using HSCL;

public class LangSellectorScript : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] TMPro.TMP_Dropdown _dropdown;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    List<Locale> _locales;

    // Unity Main Events: //////////////////////////////////////////////////////
    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        Init_dropdown();
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleCganged;
    }
    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleCganged;
    }
    
    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void OnDropdownValueChanged()
    {
        LocaleManager.Instance.SetLocaleLang((HSCL.LocaleLang)_dropdown.value);
    }
    public void OnLocaleCganged(Locale locale)
    {
        UpdateSellectorOption(locale);
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void Init_dropdown()
    {
        _locales = LocalizationSettings.AvailableLocales.Locales;
        var options = new List<TMP_Dropdown.OptionData>();
        int sellected = 0;
        for (int i = 0; i < _locales.Count; i++)
        {
            if (LocalizationSettings.SelectedLocale == _locales[i]) sellected = i;
            options.Add(new TMP_Dropdown.OptionData(_locales[i].LocaleName));
        }
        _dropdown.options = options;
        _dropdown.value = sellected;
    }

    private void UpdateSellectorOption(Locale locale)
    {
        int sellected = 0;
        for (int i = 0; i < _locales.Count; i++)
        {
            if (LocalizationSettings.SelectedLocale == _locales[i]) sellected = i;
        }
        if (_dropdown.value == sellected) return;
        else _dropdown.value = sellected;
    }

} // end of class