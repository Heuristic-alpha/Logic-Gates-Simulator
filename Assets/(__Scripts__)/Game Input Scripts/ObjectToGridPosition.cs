using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToGridPosition : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] BackGroundGridController backGroundGridController;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: ////////////////////////////////////////////////////////////// 
    [Header("Grid Placement:")]
    public bool postionToGridEnabled = false;
    public float moveSpeed = 3;
    public float distanceDelta = 0.1f;
    float _gridCellSize = 1; // most Multiple by 2

    // Unity Main Events: ////////////////////////////////////////////////////// 
    private void Awake()
    {
        SetGridCellSize(backGroundGridController.Get_CellSize());
    }

    // Unity Other Events: /////////////////////////////////////////////////////   
    // C# Public Methods: //////////////////////////////////////////////////////
    public void SetGridCellSize(float newSize) => _gridCellSize = newSize * 2;
    public Vector2 FindNearestGridCellPostion(Vector2 postion)
    {
        float halfSize = _gridCellSize / 2;
        int x1 = (int)(postion.x / _gridCellSize);
        int y1 = (int)(postion.y / _gridCellSize);
        float x2 = postion.x % _gridCellSize;
        float y2 = postion.y % _gridCellSize;

        float returnX;
        float returnY;

        if (x1 > 0) // is positive ?
        {
            if (x2 >= halfSize) returnX = (x1 + 1) * _gridCellSize;
            else returnX = x1 * _gridCellSize;
        }
        else if (x1 < 0)
        {
            if(x2 <= -halfSize) returnX = (x1 - 1)* _gridCellSize;
            else returnX = x1 * _gridCellSize;
        }
        else // x1 == 0
        {
            if (x2 >= 0)
            {
                if (x2 >= halfSize) returnX = (x1 + 1) * _gridCellSize;
                else returnX = x1 * _gridCellSize;
            }
            else // x2 is negetive
            {
                if (x2 <= -halfSize) returnX = (x1 - 1) * _gridCellSize;
                else returnX = x1 * _gridCellSize;
            }
        }

        if (y1 > 0) // is positive ?
        {
            if (y2 >= halfSize) returnY = (y1 + 1) * _gridCellSize;
            else returnY = y1 * _gridCellSize;
        }
        else if(y1 < 0)
        {
            if (y2 <= -halfSize) returnY = (y1 - 1) * _gridCellSize;
            else returnY = y1 * _gridCellSize;
        }
        else // y1 == 0
        {
            if (y2 >= 0)
            {
                if (y2 >= halfSize) returnY = (y1 + 1) * _gridCellSize;
                else returnY = y1 * _gridCellSize;
            }
            else // y2 is negetive
            {
                if (y2 <= -halfSize) returnY = (y1 - 1) * _gridCellSize;
                else returnY = y1 * _gridCellSize;
            }
        }

        return new Vector2(returnX, returnY);
    }
    public void CheckAndMoveObejectToward(Transform objectTransform)
    {
        if (postionToGridEnabled)
        {
            Vector2 target = FindNearestGridCellPostion(objectTransform.position);
            StartCoroutine(MoveObjectToPostion(objectTransform, target));
        }
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private IEnumerator MoveObjectToPostion(Transform objectTransform, Vector3 finalPostion)
    {
        IBorderable borderable = objectTransform.GetComponent<IBorderable>();
        borderable.IsMoving = true;

        while (true)
        {
            Vector2 rawDirection = finalPostion - objectTransform.position;
            Vector2 dir = rawDirection.normalized;
            Vector3 step = dir * Time.deltaTime * moveSpeed; step.z = 0;
            objectTransform.position += step;

            float dis = Vector3.Distance(finalPostion, objectTransform.position);
            if (dis <= distanceDelta)
            {
                objectTransform.position = finalPostion;
                break;
            }

            yield return null;
        }
        borderable.IsMoving = false;
    }

} // end of class