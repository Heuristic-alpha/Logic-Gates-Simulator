using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemable
{
    Item GetItem();

    void SetItem_Rotation(Item_Rotation new_item_rotation);
    Item_Rotation GetItem_Rotation();
    void RotateRight();
    void RotateLeft();

}
