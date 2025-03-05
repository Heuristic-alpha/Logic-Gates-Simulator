using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[AddComponentMenu("Localization/Asset/Localize TMP Font Event")]
public class LocalizeTMPFontEvent : LocalizedAssetEvent<TMP_FontAsset, LocalizedTMPFont, UnityEventTMPFont> { }

[Serializable]
public class LocalizedTMPFont : LocalizedAsset<TMP_FontAsset> { }

[Serializable]
public class UnityEventTMPFont : UnityEvent<TMP_FontAsset> { }
