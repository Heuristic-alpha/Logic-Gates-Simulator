using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ChipBase is a GateBase that have nameTXT.
/// </summary>
public abstract class ChipBase : GateBase
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////   
    // C# Properties: //////////////////////////////////////////////////////////       
    // C# Fields: //////////////////////////////////////////////////////////////      
    private GameObject _chipNameGameObject;

    // Unity Main Events: //////////////////////////////////////////////////////
    protected override void Awake()
    {
        base.Awake();
        Initial_chipNameGameObject();
    }

    // Unity Other Events: /////////////////////////////////////////////////////  
    // C# Public Methods: //////////////////////////////////////////////////////      
    public override void SetItem_Rotation(Item_Rotation new_item_rotation)
    {
        base.SetItem_Rotation(new_item_rotation);
        ResetWorldRotationOfChipName();
    }  
    public override void RotateRight()
    {
        base.RotateRight();
        ResetWorldRotationOfChipName();
    }
    public override void RotateLeft()
    {
        base.RotateLeft();
        ResetWorldRotationOfChipName();
    }
    
    // C# Private Methods: /////////////////////////////////////////////////////
    private void Initial_chipNameGameObject()
    {
        _chipNameGameObject = GetComponentInChildren<TextMeshProUGUI>().gameObject;
    }

    private void ResetWorldRotationOfChipName()
    {
        _chipNameGameObject.transform.rotation = Quaternion.identity;
    }

} // end of class