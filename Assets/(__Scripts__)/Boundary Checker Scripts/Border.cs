using UnityEngine;

public class Border : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] Transform up;
    [SerializeField] Transform down;
    [SerializeField] Transform right;
    [SerializeField] Transform left;

    // C# Properties: //////////////////////////////////////////////////////////
    public float Up_Y { get { return up.position.y; } }
    public float Down_Y { get { return down.position.y; } }
    public float Right_X { get { return right.position.x; } }
    public float Left_X { get { return left.position.x; } }
    public Vector2 Up_Right { get { return new Vector2(right.position.x, up.position.y); } }
    public Vector2 Up_Left { get { return new Vector2(left.position.x, up.position.y); } }
    public Vector2 Down_Right { get { return new Vector2(right.position.x, down.position.y); } }
    public Vector2 Down_Left { get { return new Vector2(left.position.x, down.position.y); } }

    public float DistanceToLeftBorder { get { return left.position.x - transform.position.x; } }
    public float DistanceToRightBorder { get { return right.position.x - transform.position.x; } }
    public float DistanceToUpBorder { get { return up.position.y - transform.position.y; } }
    public float DistanceToDownBorder { get { return down.position.y - transform.position.y; } }

    public float Height { get { return up.position.y - down.position.y; } }
    public float Width { get { return right.position.x - left.position.x; } }
    public Rect ToRect { get { return new Rect(Left_X, Up_Y, Width, Height); } }
    public Vector2 RectCenter
    {
        get
        {
            float heightOverTwo = Height / 2;
            float widthOverTwo = Width / 2;
            return new Vector2(left.position.x + widthOverTwo, down.position.y + heightOverTwo);
        }
    }

    // C# Fields: //////////////////////////////////////////////////////////////
    // Unity Main Events: //////////////////////////////////////////////////////
    private void Start()
    {
        // always set local postion to zero:
        transform.localPosition = Vector3.zero;
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    private void OnDrawGizmosSelected()
    {
        if (!up || !down || !right || !left) return;

        //creating up-left up-right down-left down-right point
        Vector2 upLeft = new Vector2(left.position.x, up.position.y);
        Vector2 upRight = new Vector2(right.position.x, up.position.y);
        Vector2 downLeft = new Vector2(left.position.x, down.position.y);
        Vector2 downRight = new Vector2(right.position.x, down.position.y);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(upLeft, upRight);
        Gizmos.DrawLine(downLeft, downRight);
        Gizmos.DrawLine(downLeft, upLeft);
        Gizmos.DrawLine(upRight, downRight);
    }
    
    // C# Public Methods: //////////////////////////////////////////////////////

    public void SetBorderUp(Vector2 new_Up)
    {
        up.localPosition = new_Up;
    }
    public void SetBorderDown(Vector2 new_Down)
    {
        down.localPosition = new_Down;
    }
    public void SetBorderRight(Vector2 new_Right)
    {
        right.localPosition = new_Right;
    }
    public void SetBorderLeft(Vector2 new_Left)
    {
        left.localPosition = new_Left;
    }
    public void RotatePoints_Plus90()
    {
        Vector3 up_LastPosition = up.position;
        Vector3 down_LastPosition = down.position;
        Vector3 right_LastPosition = right.position;
        Vector3 left_LastPosition = left.position;

        up.position = right_LastPosition;
        right.position = down_LastPosition;
        down.position = left_LastPosition;
        left.position = up_LastPosition;
    }
    public void RotatePoints_Minus90()
    {
        Vector3 up_LastPosition = up.position;
        Vector3 down_LastPosition = down.position;
        Vector3 right_LastPosition = right.position;
        Vector3 left_LastPosition = left.position;

        up.position = left_LastPosition;
        left.position = down_LastPosition;
        down.position = right_LastPosition;
        right.position = up_LastPosition;
    }

    public static void CheckAndResolve_BorderInBorder(Border inner, Border outer)
    {
    //    if(inner == null) { Debug.LogError($"inner Border in Border.CheckAndResolveBorder method is Null!"); return; }

        if (inner.Up_Y > outer.Up_Y) { inner.transform.parent.position = new Vector3(inner.transform.parent.position.x, outer.Up_Y - inner.DistanceToUpBorder, inner.transform.parent.position.z); }
        else if (inner.Down_Y < outer.Down_Y) { inner.transform.parent.position = new Vector3(inner.transform.parent.position.x, outer.Down_Y - inner.DistanceToDownBorder, inner.transform.parent.position.z); }
        if (inner.Right_X > outer.Right_X) { inner.transform.parent.position = new Vector3(outer.Right_X - inner.DistanceToRightBorder, inner.transform.parent.position.y, inner.transform.parent.position.z); }
        else if (inner.Left_X < outer.Left_X) { inner.transform.parent.position = new Vector3(outer.Left_X - inner.DistanceToLeftBorder, inner.transform.parent.position.y, inner.transform.parent.position.z); }
    }
    public static void CheckAndResolve_BorderInBackGroundBorder(Border border)
    {
        CheckAndResolve_BorderInBorder(border, GameManager.Instance.BackGroundBorder);
    }
    public static bool IsThePointInsideBorder(Vector2 point,Border border)
    {
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;

        if (point.x < border.Right_X) flag1 = true;
        if (point.x > border.Left_X) flag2 = true;
        if (point.y < border.Up_Y) flag3 = true;
        if (point.y > border.Down_Y) flag4 = true;

        if (flag1 && flag2 && flag3 && flag4) return true;
        else return false;
    }
    public static bool IsThePointInsideBackGroundBorder(Vector2 point)
    {
        return IsThePointInsideBorder(point, GameManager.Instance.BackGroundBorder);
    }
    public static bool IsBordersInCollision(Border A, Border B)
    {
        return B.Left_X <= A.Right_X && B.Right_X >= A.Left_X && B.Up_Y >= A.Down_Y && B.Down_Y <= A.Up_Y;
    }
    public static void CheckAndResolve_BorderOutOfBorder(Border staticBorder, Border dynamicBorder)
    {
        // flags: flags to record if borders edges is overlapped with other border
        bool dB_UpRight;
        bool dB_UpLeft;
        bool dB_DownRight;      
        bool dB_DownLeft;
        bool sB_UpRight = false;
        bool sB_UpLeft = false;
        bool sB_DownRight = false;
        bool sB_DownLeft = false;
        bool shouldOperateWithDynamicBorder;

        dB_UpRight = IsThePointInsideBorder(dynamicBorder.Up_Right, staticBorder);
        dB_UpLeft = IsThePointInsideBorder(dynamicBorder.Up_Left, staticBorder);
        dB_DownRight = IsThePointInsideBorder(dynamicBorder.Down_Right, staticBorder);
        dB_DownLeft = IsThePointInsideBorder(dynamicBorder.Down_Left, staticBorder);

        // if dynamicBorder's edges dont overlapped with staticBorder,
        // checks overlapping of staticBorder edges with dynamicBorder.
        if(!dB_UpRight && !dB_UpLeft && !dB_DownRight && !dB_DownLeft)
        {
            shouldOperateWithDynamicBorder = false;

            sB_UpRight = IsThePointInsideBorder(staticBorder.Up_Right, dynamicBorder);
            sB_UpLeft = IsThePointInsideBorder(staticBorder.Up_Left, dynamicBorder);
            sB_DownRight = IsThePointInsideBorder(staticBorder.Down_Right, dynamicBorder);
            sB_DownLeft = IsThePointInsideBorder(staticBorder.Down_Left, dynamicBorder);

            if (!sB_UpRight && !sB_UpLeft && !sB_DownRight && !sB_DownLeft)// collision not happend
            {
              //  Debug.Log($"border collision not happend Between <color=red>{dynamicBorder.transform.parent.name} </color> and <color=red>{staticBorder.transform.parent.name}</color>");
                return;
            }
        }
        else
        {
            shouldOperateWithDynamicBorder = true;
        }


        if (shouldOperateWithDynamicBorder)
        {
            // check collision of dynamicBorder Up:
            if(dB_UpRight && dB_UpLeft && !dB_DownRight && !dB_DownLeft)
            {
                float dist = staticBorder.Down_Y - dynamicBorder.Up_Y;
                float ABSdist = Mathf.Abs(dist);
                dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                      dynamicBorder.transform.parent.position.y - ABSdist,
                                                                      dynamicBorder.transform.parent.position.z);
                return;
            }

            // check collision of dynamicBorder Down:
            else if (!dB_UpRight && !dB_UpLeft && dB_DownRight && dB_DownLeft)
            {
                float dist = staticBorder.Up_Y - dynamicBorder.Down_Y;
                float ABSdist = Mathf.Abs(dist);
                dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                      dynamicBorder.transform.parent.position.y + ABSdist,
                                                                      dynamicBorder.transform.parent.position.z);
                return;
            }

            // check collision of dynamicBorder Right:
            else if (dB_UpRight && !dB_UpLeft && dB_DownRight && !dB_DownLeft)
            {
                float dist = staticBorder.Left_X - dynamicBorder.Right_X;
                float ABSdist = Mathf.Abs(dist);
                dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x - ABSdist,
                                                                      dynamicBorder.transform.parent.position.y,
                                                                      dynamicBorder.transform.parent.position.z);        
                return;
            }

            // check collision of dynamicBorder Left:
            else if (!dB_UpRight && dB_UpLeft && !dB_DownRight && dB_DownLeft)
            {
                float dist = staticBorder.Right_X - dynamicBorder.Left_X;
                float ABSdist = Mathf.Abs(dist);
                dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x + ABSdist,
                                                                      dynamicBorder.transform.parent.position.y,
                                                                      dynamicBorder.transform.parent.position.z);
                return;
            }

            // ------------------------------------------------ //

            // check collision of dynamicBorder UpRight:
            else if (dB_UpRight && !dB_UpLeft && !dB_DownRight && !dB_DownLeft)
            {
                Vector2 dist = staticBorder.Down_Left - dynamicBorder.Up_Right;
                float ABS_X = Mathf.Abs(dist.x);
                float ABS_Y = Mathf.Abs(dist.y);

                if(ABS_X < ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x - ABS_X,
                                                                          dynamicBorder.transform.parent.position.y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else if(ABS_X > ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                          dynamicBorder.transform.parent.position.y  - ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else // ABS_X == ABS_Y
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x - ABS_X,
                                                                          dynamicBorder.transform.parent.position.y - ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }

                return;
            }

            // check collision of dynamicBorder UpLeft:
            else if (!dB_UpRight && dB_UpLeft && !dB_DownRight && !dB_DownLeft)
            {
                Vector2 dist = staticBorder.Down_Right - dynamicBorder.Up_Left;
                float ABS_X = Mathf.Abs(dist.x);
                float ABS_Y = Mathf.Abs(dist.y);

                if (ABS_X < ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x + ABS_X,
                                                                          dynamicBorder.transform.parent.position.y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else if (ABS_X > ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                          dynamicBorder.transform.parent.position.y - ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else // ABS_X == ABS_Y
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x + ABS_X,
                                                                          dynamicBorder.transform.parent.position.y - ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }

                return;
            }

            // check collision of dynamicBorder DownRight:
            else if (!dB_UpRight && !dB_UpLeft && dB_DownRight && !dB_DownLeft)
            {
                Vector2 dist = staticBorder.Up_Left - dynamicBorder.Down_Right;
                float ABS_X = Mathf.Abs(dist.x);
                float ABS_Y = Mathf.Abs(dist.y);

                if (ABS_X < ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x - ABS_X,
                                                                          dynamicBorder.transform.parent.position.y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else if (ABS_X > ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                          dynamicBorder.transform.parent.position.y + ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else // ABS_X == ABS_Y
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x - ABS_X,
                                                                          dynamicBorder.transform.parent.position.y + ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }

                return;
            }

            // check collision of dynamicBorder DownLeft:
            else if (!dB_UpRight && !dB_UpLeft && !dB_DownRight && dB_DownLeft)
            {
                Vector2 dist = staticBorder.Up_Right - dynamicBorder.Down_Left;
                float ABS_X = Mathf.Abs(dist.x);
                float ABS_Y = Mathf.Abs(dist.y);

                if (ABS_X < ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x + ABS_X,
                                                                          dynamicBorder.transform.parent.position.y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else if (ABS_X > ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                          dynamicBorder.transform.parent.position.y + ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else // ABS_X == ABS_Y
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x + ABS_X,
                                                                          dynamicBorder.transform.parent.position.y + ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }

                return;
            }

            // check if dynamicBorder is inside staticBorder:
            else if (dB_UpRight && dB_UpLeft && dB_DownRight && dB_DownLeft)
            {
                Vector2 dir = dynamicBorder.transform.parent.position - staticBorder.transform.parent.position;
                Ray2D ray = new Ray2D(dynamicBorder.transform.parent.position, dir);

                Vector2 hitPoint;
                if (RayExtention.GetIntersectionOfRayInBorder(in ray, staticBorder, out hitPoint))
                {
                    dynamicBorder.transform.parent.position = new Vector3(hitPoint.x, hitPoint.y, dynamicBorder.transform.parent.position.z);
                    CheckAndResolve_BorderOutOfBorder(staticBorder, dynamicBorder);
                }

                return;
            }
        }
        else // (shouldOperateWithDynamicBorder == false) ---------------------------------------------------------------------------------
        {
            // check collision of staticBorder Up:
            if (sB_UpRight && sB_UpLeft && !sB_DownRight && !sB_DownLeft)
            {
                float dist = dynamicBorder.Down_Y - staticBorder.Up_Y;
                float ABSdist = Mathf.Abs(dist);
                dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                      dynamicBorder.transform.parent.position.y + ABSdist,
                                                                      dynamicBorder.transform.parent.position.z);
                return;
            }

            // check collision of staticBorder Down:
            else if (!sB_UpRight && !sB_UpLeft && sB_DownRight && sB_DownLeft)
            {
                float dist = dynamicBorder.Up_Y - staticBorder.Down_Y;
                float ABSdist = Mathf.Abs(dist);
                dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                      dynamicBorder.transform.parent.position.y - ABSdist,
                                                                      dynamicBorder.transform.parent.position.z);
                return;
            }

            // check collision of staticBorder Right:
            else if (sB_UpRight && !sB_UpLeft && sB_DownRight && !sB_DownLeft)
            {
                float dist = dynamicBorder.Left_X - staticBorder.Right_X;
                float ABSdist = Mathf.Abs(dist);
                dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x + ABSdist,
                                                                      dynamicBorder.transform.parent.position.y,
                                                                      dynamicBorder.transform.parent.position.z);
                return;
            }

            // check collision of staticBorder Left:
            else if (!sB_UpRight && sB_UpLeft && !sB_DownRight && sB_DownLeft)
            {
                float dist = dynamicBorder.Right_X - staticBorder.Left_X;
                float ABSdist = Mathf.Abs(dist);
                dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x - ABSdist,
                                                                      dynamicBorder.transform.parent.position.y,
                                                                      dynamicBorder.transform.parent.position.z);
                return;
            }

            // ------------------------------------------------ //

            // check collision of staticBorder UpRight:
            else if (sB_UpRight && !sB_UpLeft && !sB_DownRight && !sB_DownLeft)
            {
                Vector2 dist = dynamicBorder.Down_Left - staticBorder.Up_Right;
                float ABS_X = Mathf.Abs(dist.x);
                float ABS_Y = Mathf.Abs(dist.y);

                if (ABS_X < ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x + ABS_X,
                                                                          dynamicBorder.transform.parent.position.y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else if (ABS_X > ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                          dynamicBorder.transform.parent.position.y + ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else // ABS_X == ABS_Y
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x + ABS_X,
                                                                          dynamicBorder.transform.parent.position.y + ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }

                return;
            }

            // check collision of staticBorder UpLeft:
            else if (!sB_UpRight && sB_UpLeft && !sB_DownRight && !sB_DownLeft)
            {
                Vector2 dist = dynamicBorder.Down_Right - staticBorder.Up_Left;
                float ABS_X = Mathf.Abs(dist.x);
                float ABS_Y = Mathf.Abs(dist.y);

                if (ABS_X < ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x - ABS_X,
                                                                          dynamicBorder.transform.parent.position.y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else if (ABS_X > ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                          dynamicBorder.transform.parent.position.y + ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);

                    Debug.Log("dB_go up 2");
                }
                else // ABS_X == ABS_Y
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x - ABS_X,
                                                                          dynamicBorder.transform.parent.position.y + ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);

                    Debug.Log("dB_go up left 2");
                }

                return;
            }

            // check collision of staticBorder DownRight:
            else if (!dB_UpRight && !dB_UpLeft && dB_DownRight && !dB_DownLeft)
            {
                Vector2 dist = dynamicBorder.Up_Left - staticBorder.Down_Right;
                float ABS_X = Mathf.Abs(dist.x);
                float ABS_Y = Mathf.Abs(dist.y);

                if (ABS_X < ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x + ABS_X,
                                                                          dynamicBorder.transform.parent.position.y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else if (ABS_X > ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                          dynamicBorder.transform.parent.position.y - ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else // ABS_X == ABS_Y
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x + ABS_X,
                                                                          dynamicBorder.transform.parent.position.y - ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }

                return;
            }

            // check collision of staticBorder DownLeft:
            else if (!dB_UpRight && !dB_UpLeft && !dB_DownRight && dB_DownLeft)
            {
                Vector2 dist = dynamicBorder.Up_Right - staticBorder.Down_Left;
                float ABS_X = Mathf.Abs(dist.x);
                float ABS_Y = Mathf.Abs(dist.y);

                if (ABS_X < ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x - ABS_X,
                                                                          dynamicBorder.transform.parent.position.y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else if (ABS_X > ABS_Y)
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x,
                                                                          dynamicBorder.transform.parent.position.y - ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }
                else // ABS_X == ABS_Y
                {
                    dynamicBorder.transform.parent.position = new Vector3(dynamicBorder.transform.parent.position.x - ABS_X,
                                                                          dynamicBorder.transform.parent.position.y - ABS_Y,
                                                                          dynamicBorder.transform.parent.position.z);
                }

                return;
            }

            // check if staticBorder is inside dynamicBorder:
            else if (sB_UpRight && sB_UpLeft && sB_DownRight && sB_DownLeft)
            {
                Vector2 dir = staticBorder.transform.parent.position - dynamicBorder.transform.parent.position;
                Ray2D ray = new Ray2D(staticBorder.transform.parent.position, dir);

                Vector2 hitPoint;
                if (RayExtention.GetIntersectionOfRayInBorder(in ray, dynamicBorder, out hitPoint))
                {
                    Vector3 tempDir = hitPoint - (Vector2)dynamicBorder.transform.parent.position;
                    dynamicBorder.transform.parent.position = staticBorder.transform.parent.position - tempDir;
                    CheckAndResolve_BorderOutOfBorder(staticBorder, dynamicBorder);
                }

                return;
            }
        }
    }

    public static explicit operator Rect(Border border)
    {
        return new Rect(border.Left_X, border.Up_Y, border.right.position.x - border.left.position.x, border.up.position.y - border.down.position.y);
    }

    // C# Private Methods: /////////////////////////////////////////////////////
}