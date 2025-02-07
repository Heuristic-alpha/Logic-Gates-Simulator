using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSellectorController : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject itemPrefab;
    [SerializeField] GameObject itemsParent;   
    [SerializeField, Header("Items to Instantiate:")] Item[] itemsToInstantiate;


    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] ItemPlacementController itemPlacementController2;
    [SerializeField] ScrollRect scrollRect;  

    // C# Properties: //////////////////////////////////////////////////////////
    public int ItemGroupIndex { get { return itemGroupIndex; } }

    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] int itemGroupIndex;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        GenerateItems();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    // C# Private Methods: /////////////////////////////////////////////////////
    private void GenerateItems()
    {
        for (int i = 0; i < itemsToInstantiate.Length; i++)
        {
            GameObject currentItem = Instantiate(itemPrefab, itemsParent.transform, false);
            UI_Item current_Ui_Item = currentItem.GetComponent<UI_Item>();
            Item cur_Item = itemsToInstantiate[i];
            current_Ui_Item.Initialize(cur_Item, cur_Item.Display_Name, cur_Item.Icon, cur_Item.Prefab, itemPlacementController2, scrollRect);
        }
    }
    

} // end of class