using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_Item : MonoBehaviour , IBeginDragHandler , IEndDragHandler , IDragHandler , IPointerEnterHandler , IPointerExitHandler
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////  
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Image iconImage;
    Image backGroundImage;
    GameObject itemPrefab;
    ItemPlacementController itemPlacementController2;

    ScrollRect parentObjectScrolRect;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    private Item item_scriptableObject;

    Color normalColor;
    [SerializeField] Color hoverColor;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        backGroundImage = GetComponent<Image>();
        normalColor = backGroundImage.color;      
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void Initialize(Item item,string name , Sprite icon , GameObject prefab, ItemPlacementController itemPlacementController2, ScrollRect scrollRect)
    {
        item_scriptableObject = item;
        itemNameText.text = name;
        iconImage.sprite= icon;
        itemPrefab = prefab;
        this.itemPlacementController2 = itemPlacementController2;
        parentObjectScrolRect = scrollRect;
    }   
  
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemPlacementController2.SetCurrentItem(item_scriptableObject);
        itemPlacementController2.OnBeginDragOnUIItem();

        //pass event data to scrolRect Ui:
        parentObjectScrolRect.OnBeginDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        parentObjectScrolRect.OnDrag(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        parentObjectScrolRect.OnEndDrag(eventData);
        itemPlacementController2.OnEndDragOnUIItem();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        backGroundImage.color = hoverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        backGroundImage.color = normalColor;       
    }



    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class