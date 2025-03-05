using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using HSCL;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(RTLTextMeshPro), typeof(LocalizeStringEvent))]
public class RTLMproHelper : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    RTLTextMeshPro _RTLTpro;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] List<TMP_FontAsset> _TMP_FontAssets;
    [SerializeField] List<bool> _RTL_Enabled;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        int listSize = LocalizationSettings.AvailableLocales.Locales.Count;
        _RTLTpro = GetComponent<RTLTextMeshPro>();
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    // C# Private Methods: /////////////////////////////////////////////////////
    private void OnLocaleChanged(Locale newlocal)
    {
        HSCL.LocaleLang currentLocal = LocaleManager.Instance.GetLocaleLang();
        _RTLTpro.font = _TMP_FontAssets[(int)currentLocal];
        _RTLTpro.Farsi = _RTL_Enabled[(int)currentLocal];
    }

} // end of class