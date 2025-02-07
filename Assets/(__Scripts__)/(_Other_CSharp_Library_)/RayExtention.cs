
using UnityEngine;

public static class RayExtention
{
    private const float Epsilon = 0.00000001f;
    public static bool GetIntersectionOfRayToLine(in Ray2D ray, in LineSegment line , out Vector2 intersectionPoint)
    {
        Vector2 p0 = ray.origin;
        Vector2 p1 = p0 + ray.direction * 10000;
        Vector2 p2 = line.p1;
        Vector2 p3 = line.p2;

        Vector2 s1 = p1 - p0;
        Vector2 s2 = p3 - p2;

        float v1 = (-s2.x * s1.y + s1.x * s2.y);

        if(Mathf.Abs(v1) < Epsilon)
        {
            intersectionPoint = Vector2.zero;
            return false;
        }

        float s = (-s1.y * (p0.x - p2.x) + s1.x * (p0.y - p2.y)) / v1;
        float t = (s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) / v1;

        if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
        {
            intersectionPoint = p0 + t * s1;
            return true;
        }
        else
        {
            intersectionPoint = Vector2.zero;
            return false;
        }
    }
    public static bool GetIntersectionOfRayToLine(in Ray2D ray, in LineSegment line, out Vector2 intersectionPoint, float rayLength = 10000)
    {
        Vector2 p0 = ray.origin;
        Vector2 p1 = p0 + ray.direction * rayLength;
        Vector2 p2 = line.p1;
        Vector2 p3 = line.p2;

        Vector2 s1 = p1 - p0;
        Vector2 s2 = p3 - p2;

        float dot = (-s2.x * s1.y + s1.x * s2.y);

        if (Mathf.Abs(dot) < Epsilon)
        {
            intersectionPoint = Vector2.zero;
            return false;
        }

        float s = (-s1.y * (p0.x - p2.x) + s1.x * (p0.y - p2.y)) / dot;
        float t = (s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) / dot;

        if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
        {
            intersectionPoint = p0 + t * s1;
            return true;
        }
        else
        {
            intersectionPoint = Vector2.zero;
            return false;
        }
    }
    public static bool GetIntersectionOfLineToLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, out Vector2 intersectionPoint)
    {     
        Vector2 s1 = p1 - p0;
        Vector2 s2 = p3 - p2;

        float v1 = (-s2.x * s1.y + s1.x * s2.y);

        if (Mathf.Abs(v1) < Epsilon)
        {
            intersectionPoint = Vector2.zero;
            return false;
        }

        float s = (-s1.y * (p0.x - p2.x) + s1.x * (p0.y - p2.y)) / v1;
        float t = (s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) / v1;

        if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
        {
            intersectionPoint = p0 + t * s1;
            return true;
        }
        else
        {
            intersectionPoint = Vector2.zero;
            return false;
        }
    }
    public static bool GetIntersectionOfRayInBorder(in Ray2D ray, Border border, out Vector2 intersectionPoint)
    {
        // flags
        bool is_Y_biggerThan_X = false;
        bool is_Y_equalTo_X = false;

        float ABS_x = Mathf.Abs(ray.direction.x);
        float ABS_y = Mathf.Abs(ray.direction.y);
        float Dis_xy = Mathf.Abs(ABS_y - ABS_x);

        if (ABS_y > ABS_x && Dis_xy > Epsilon) is_Y_biggerThan_X = true;
        else if (ABS_y < ABS_x && Dis_xy > Epsilon) is_Y_biggerThan_X = false;
        else { is_Y_equalTo_X = true; }

        if (is_Y_equalTo_X)
        {
            // Up Right corner of rect
            if (ray.direction.x > 0 && ray.direction.y > 0)
            {
                intersectionPoint = border.Up_Right;
                return true;
            }
            // Up left corner of rect
            if (ray.direction.x < 0 && ray.direction.y > 0)
            {
                intersectionPoint = border.Up_Left;
                return true;
            }
            // down Right corner of rect
            if (ray.direction.x > 0 && ray.direction.y < 0)
            {
                intersectionPoint = border.Down_Right;
                return true;
            }
            // down left corner of rect
            if (ray.direction.x > 0 && ray.direction.y < 0)
            {
                intersectionPoint = border.Down_Left;
                return true;
            }
            // error
            else
            {
                intersectionPoint = Vector2.zero;
                return false;
            }
        }
        else
        {
            // up direction
            if (is_Y_biggerThan_X && ray.direction.y > 0)
            {
                LineSegment line = new LineSegment(border.Up_Left, border.Up_Right);
                if (GetIntersectionOfRayToLine(in ray, in line, out intersectionPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // down direction
            else if (is_Y_biggerThan_X && ray.direction.y < 0)
            {
                LineSegment line = new LineSegment(border.Down_Left, border.Down_Right);              
                if (GetIntersectionOfRayToLine(in ray, in line, out intersectionPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // right direction
            else if (!is_Y_biggerThan_X && ray.direction.x > 0)
            {
                LineSegment line = new LineSegment(border.Up_Right, border.Down_Right);           
                if (GetIntersectionOfRayToLine(in ray, in line, out intersectionPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // left direction
            else if (!is_Y_biggerThan_X && ray.direction.x < 0)
            {
                LineSegment line = new LineSegment(border.Down_Left, border.Up_Left);   
                if (GetIntersectionOfRayToLine(in ray, in line, out intersectionPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // error
            else
            {
                intersectionPoint = Vector2.zero;
                return false;
            }
        }

    }
    public static bool GetIntersectionOfRayInRect(in Ray2D ray,in Rect rect, out Vector2 intersectionPoint)
    {
        // flags
        bool is_Y_biggerThan_X = false;
        bool is_Y_equalTo_X = false;

        float ABS_x = Mathf.Abs(ray.direction.x);
        float ABS_y = Mathf.Abs(ray.direction.y);
        float Dis_xy = Mathf.Abs(ABS_y - ABS_x);

        if (ABS_y > ABS_x && Dis_xy > Epsilon) is_Y_biggerThan_X = true;
        else if (ABS_y < ABS_x && Dis_xy > Epsilon) is_Y_biggerThan_X = false;
        else { is_Y_equalTo_X = true; }

        if (is_Y_equalTo_X)
        {
            // Up Right corner of rect
            if (ray.direction.x > 0 && ray.direction.y > 0)
            {
                intersectionPoint = new Vector2(rect.x + rect.width, rect.y);
                return true;
            }
            // Up left corner of rect
            if (ray.direction.x < 0 && ray.direction.y > 0)
            {
                intersectionPoint = rect.position;
                return true;
            }
            // down Right corner of rect
            if (ray.direction.x > 0 && ray.direction.y < 0)
            {
                intersectionPoint = new Vector2(rect.x + rect.width, rect.y - rect.height);
                return true;
            }
            // down left corner of rect
            if (ray.direction.x > 0 && ray.direction.y < 0)
            {
                intersectionPoint = new Vector2(rect.x, rect.y - rect.height);
                return true;
            }
            // error
            else
            {
                intersectionPoint = Vector2.zero;
                return false;
            }
        }
        else
        {
            // up direction
            if (is_Y_biggerThan_X && ray.direction.y > 0)
            {
                LineSegment line = new LineSegment(rect.position, new Vector2(rect.x + rect.width, rect.y));
                if (GetIntersectionOfRayToLine(in ray, in line, out intersectionPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // down direction
            else if (is_Y_biggerThan_X && ray.direction.y < 0)
            {
                LineSegment line = new LineSegment(new Vector2(rect.x, rect.y - rect.height), new Vector2(rect.x + rect.width, rect.y - rect.height));
                if (GetIntersectionOfRayToLine(in ray, in line, out intersectionPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // right direction
            else if (!is_Y_biggerThan_X && ray.direction.x > 0)
            {
                LineSegment line = new LineSegment(new Vector2(rect.x+rect.width, rect.y), new Vector2(rect.x + rect.width, rect.y - rect.height));
                if (GetIntersectionOfRayToLine(in ray, in line, out intersectionPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // left direction
            else if (!is_Y_biggerThan_X && ray.direction.x < 0)
            {
                LineSegment line = new LineSegment(rect.position, new Vector2(rect.x, rect.y - rect.height));
                if (GetIntersectionOfRayToLine(in ray, in line, out intersectionPoint))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // error
            else
            {
                intersectionPoint = Vector2.zero;
                return false;
            }
        }

    }
}

public readonly struct LineSegment
{
    public readonly Vector2 p1;
    public readonly Vector2 p2;

    public LineSegment(Vector2 p1, Vector2 p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }
}