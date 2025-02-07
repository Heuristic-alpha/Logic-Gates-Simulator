using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;

[CreateAssetMenu(fileName = "Item Scriptable Object", menuName = "Item Scriptable Object", order = 1)]
public class Item : ScriptableObject
{
    [SerializeField] int id = -1;
    [SerializeField] string display_name;
    [SerializeField] Sprite icon;
    [SerializeField] GameObject prefab;

    public int Id { get { return id; } }
    public string Display_Name { get { return display_name; } }
    public Sprite Icon { get { return icon; } }
    public GameObject Prefab { get { return prefab; } }

    public void Set_Id(int index) => id = index;
 
    public static Item[] Items;

}

