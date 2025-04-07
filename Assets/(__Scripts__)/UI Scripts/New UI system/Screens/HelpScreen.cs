using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using RTLTMPro;

public class HelpScreen : WindowScreenBase
{
    public GameObject ContentHolder;
    private RTLTextMeshPro[] _ArrayOfRTL;
    private List<Locale> _locales;

    public override void OnCreate()
    {
        base.OnCreate();
        _screenSample = HSCL.ScreenSample.HelpScreen;
        _ArrayOfRTL = ContentHolder.transform.GetComponentsInChildren<RTLTextMeshPro>();
       // Debug.Log($"HelpScreen: found {_ArrayOfRTL.Length} RTL Childs.");
        _locales = LocalizationSettings.AvailableLocales.Locales;
        LocalizationSettings.SelectedLocaleChanged += OnLocaleCganged;
        OnLocaleCganged(LocalizationSettings.SelectedLocale); // run and fresh foe the first time
    }

    public void OnLocaleCganged(Locale locale)
    {
        if (locale == _locales[1]) // persian lang is a RTL
        {
            foreach(RTLTextMeshPro rTLText in _ArrayOfRTL)
            {
                rTLText.alignment = TMPro.TextAlignmentOptions.MidlineRight;
                rTLText.Farsi = true;
            }
        }
        else // for English that is LTR and others
        {
            foreach (RTLTextMeshPro rTLText in _ArrayOfRTL)
            {
                rTLText.alignment = TMPro.TextAlignmentOptions.MidlineJustified;
                rTLText.Farsi = false;
            }
        }
    }
   
}
