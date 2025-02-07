using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowButtonController : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject itemPlacementHolder;

    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] Image image;
    [SerializeField] Sprite leftArrow;
    [SerializeField] Sprite rightArrow;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        Refresh();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void OnButtonClicked()
    {
        itemPlacementHolder.SetActive(!itemPlacementHolder.activeSelf);
        Refresh();
    }
    public void OnPointerEnter()
    {
        image.color = Color.black;
    }
    public void OnPointerExit()
    {
        image.color= Color.white;
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void Refresh()
    {
        if(itemPlacementHolder.activeSelf == true)
        {
            image.sprite = rightArrow;
        }
        else
        {
            image.sprite = leftArrow;
        }
    }

} // end of class