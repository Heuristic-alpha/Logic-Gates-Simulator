using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace HSCL
{

    /// <summary>
    /// locale manager is a singeleton object that provide access to localization setting in runtime
    /// </summary>
    public class LocaleManager : MonoBehaviour
    {
        // singeleton instance
        public static LocaleManager Instance { get; private set; }

        LocaleLang localeLang;
        private List<Locale> locales;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
            
        }

        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            locales = LocalizationSettings.AvailableLocales.Locales;
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        private void OnLocaleChanged(UnityEngine.Localization.Locale newLocale)
        {
            UpdateInspectorProperties(newLocale);
        }

        private void UpdateInspectorProperties(UnityEngine.Localization.Locale newLocale)
        {
            for (int i = 0; i < locales.Count; i++)
            {
                if (newLocale == locales[i]) localeLang = (HSCL.LocaleLang)i;
            }
        }

        public void SetLocaleLang(HSCL.LocaleLang localeLang)
        {
            this.localeLang = localeLang;
            switch (localeLang)
            {
                case LocaleLang.English:
                    LocalizationSettings.SelectedLocale = locales[0];
                    break;

                case LocaleLang.Persian:
                    LocalizationSettings.SelectedLocale = locales[1];
                    break;
            }
        }

        public HSCL.LocaleLang GetLocaleLang() => localeLang;

    }

    [Serializable]
    public enum LocaleLang
    {
        English = 0,
        Persian = 1,
    }
}
