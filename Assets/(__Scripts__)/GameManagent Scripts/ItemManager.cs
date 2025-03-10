using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject _items_holder;
    [SerializeField] GameObject _wires_holder;
    [SerializeField] GameObject _wirePrefab;

    // Unity Components: ///////////////////////////////////////////////////////
    // C# Properties: //////////////////////////////////////////////////////////
    public static ItemManager Singeleton { get { return _itemManager; } }

    // C# Fields: //////////////////////////////////////////////////////////////
    private static ItemManager _itemManager;
    [SerializeField] Item[] _Items;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        //singeleton:
        if (_itemManager != null) Destroy(gameObject);
        else { _itemManager = this; }
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public Item GetItem(int id)
    {
        if (id < 0 || id >= _Items.Length)
        {
            Debug.LogError("id is out of range.");
            return null;
        }
        return _Items[id];
    }
    public Item StartItem() => _Items[0];
    public Item LastItem() => _Items[_Items.Length - 1];
    public int LastItemID() => _Items.Length - 1;

    public GameObject AddItemAndReturnGameObject(int itemId, Vector2 postion, Item_Rotation item_Rotation)
    {
        GameObject prefab = GetItem(itemId).Prefab;
        GameObject spawnedItem = Instantiate(prefab, postion, Quaternion.identity);
        spawnedItem.transform.parent = _items_holder.transform;
        IItemable IItem = spawnedItem.GetComponent<IItemable>();
        IItem.SetItem_Rotation(item_Rotation);
        spawnedItem.name = IItem.GetItem().Display_Name;

        return spawnedItem;
    }
    public void AddItem(int itemId, Vector2 postion, Item_Rotation item_Rotation)
    {
        GameObject prefab = GetItem(itemId).Prefab;
        GameObject spawnedItem = Instantiate(prefab, postion, Quaternion.identity);
        spawnedItem.transform.parent = _items_holder.transform;
        IItemable IItem = spawnedItem.GetComponent<IItemable>();
        IItem.SetItem_Rotation(item_Rotation);
        spawnedItem.name = IItem.GetItem().Display_Name;
    }   
    public void AddItem(ItemInfo itemInfo)
    {
        AddItem(itemInfo.id, new Vector2(itemInfo.x, itemInfo.y), itemInfo.item_Rotation);
    }
    public void AddItems(ItemInfo[] itemInfos)
    {
        for (int i = 0; i < itemInfos.Length; i++)
        {
            AddItem(itemInfos[i]);
        }
    }
    public void RemoveItem(GameObject go)
    {
        Destroy(go);
    }
    public void AddWire(GameObject startPoint, GameObject endPoint)
    {
        GameObject spawnedWire = Instantiate(_wirePrefab, Vector3.zero, Quaternion.identity);
        spawnedWire.name = "Wire";
        spawnedWire.transform.parent = _wires_holder.transform;
        Wire wire = spawnedWire.GetComponent<Wire>();
        wire.StartPoint = startPoint;
        wire.EndPoint = endPoint;
        startPoint.GetComponent<IWirable>().Wire_GameObject = spawnedWire;
        endPoint.GetComponent<IWirable>().Wire_GameObject = spawnedWire;
    }
    public void AddWire(WireInfo wireInfo)
    {
        GameObject startPoint;
        GameObject endPoint;
        GameObject go1 = GetSpawnedItem(wireInfo.localItemId_s);
        GameObject go2 = GetSpawnedItem(wireInfo.localItemId_e);

        if (wireInfo.pinType_s == Trait.Output)
        {
            startPoint = go1.GetComponent<PinManager>().GetOutputPin(wireInfo.pinIndex_s);
        }
        else if (wireInfo.pinType_s == Trait.Input)
        {
            startPoint = go1.GetComponent<PinManager>().GetInputPin(wireInfo.pinIndex_s);
        }
        else startPoint = null;// error

        if (wireInfo.pinType_e == Trait.Output)
        {
            endPoint = go2.GetComponent<PinManager>().GetOutputPin(wireInfo.pinIndex_e);
        }
        else if (wireInfo.pinType_e == Trait.Input)
        {
            endPoint = go2.GetComponent<PinManager>().GetInputPin(wireInfo.pinIndex_e);
        }
        else endPoint = null;// error

        AddWire(startPoint, endPoint);
    }
    public void AddWires(WireInfo[] wireInfos)
    {
        for (int i = 0; i < wireInfos.Length; i++)
        {
            AddWire(wireInfos[i]);
        }
    }

    public void RemoveAllSpawnedItems()
    {
        var spawnedItems = GetAllSpawnedItems();
        foreach (var spawnedItem in spawnedItems) { DestroyImmediate(spawnedItem.gameObject); }
    }
    public void RemoveAllSpawnedWires()
    {
        var spawnedWires = GetAllSpawnedWires();
        foreach (var spawnedWire in spawnedWires) { DestroyImmediate(spawnedWire.gameObject); }
    }
    public void RemoveAllSpawned()
    {
        RemoveAllSpawnedItems();
        RemoveAllSpawnedWires();
    }

    public GameObject GetSpawnedItem(int index) => _items_holder.transform.GetChild(index).gameObject;
    public GameObject GetSpawnedWire(int index) => _wires_holder.transform.GetChild(index).gameObject;
    public GameObject[] GetAllSpawnedItems()
    {
        int gameObjectCounts = _items_holder.transform.childCount;
        GameObject[] spawnedItems = new GameObject[gameObjectCounts];
        for (int i = 0; i < gameObjectCounts; i++)
        {
            spawnedItems[i] = _items_holder.transform.GetChild(i).gameObject;
        }
        return spawnedItems;
    }
    public GameObject[] GetAllSpawnedWires()
    {
        int gameObjectCounts = _wires_holder.transform.childCount;
        GameObject[] spawnedWires = new GameObject[gameObjectCounts];
        for (int i = 0; i < gameObjectCounts; i++)
        {
            spawnedWires[i] = _wires_holder.transform.GetChild(i).gameObject;
        }
        return spawnedWires;
    }

    public ItemInfo[] GetAllSpawnedItemsAsItemInfoes()
    {
        GameObject[] spawnedItems = GetAllSpawnedItems();
        List<ItemInfo> itemInfos = new List<ItemInfo>();
        for (int i = 0; i < spawnedItems.Length; i++)
        {
            itemInfos.Add(ItemInfo.ToItemInfoFrom(spawnedItems[i].GetComponent<IItemable>()));
        }
        return itemInfos.ToArray();
    }
    public WireInfo[] GetAllSpawnedWiresAsWireInfoes()
    {
        GameObject[] spawnedWires = GetAllSpawnedWires();
        GameObject[] spawnedItems = GetAllSpawnedItems();
        List<WireInfo> wireInfos = new List<WireInfo>();

        for (int i = 0; i < spawnedWires.Length; i++)
        {
            wireInfos.Add(spawnedWires[i].GetComponent<Wire>().ToWireInfo(spawnedItems));
        }
        return wireInfos.ToArray();
    }

    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class

[Serializable]
public enum Item_Rotation
{
    n, // north
    e, // east
    s, // south
    w  // west
}