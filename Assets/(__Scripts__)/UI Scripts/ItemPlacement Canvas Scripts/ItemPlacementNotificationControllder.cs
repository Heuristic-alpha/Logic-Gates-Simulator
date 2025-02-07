using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacementNotificationControllder : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] TMPro.TextMeshProUGUI _bannerText;
    [SerializeField] UnityEngine.UI.Image _image;
    private Item _currentItem;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    // Unity Main Events: //////////////////////////////////////////////////////
    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void Init(Item item)
    {
        _image.sprite = item.Icon;
        _bannerText.text = $"placing {item.Display_Name} ";
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class