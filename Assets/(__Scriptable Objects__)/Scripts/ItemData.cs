using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data Scriptable Object" , menuName = "Item Data Scriptable Object", order = 0)]
public class ItemData : ScriptableObject
{
    public string itemName = "name";
    public Sprite itemIcon= null;
    public GameObject itemPrefab= null;
}
