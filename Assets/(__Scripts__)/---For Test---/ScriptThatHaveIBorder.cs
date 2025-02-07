using UnityEngine;

public class ScriptThatHaveIBorder : MonoBehaviour, IBorderable
{

    [SerializeField] Border borderA;
    [SerializeField] Border borderB;

    public bool IsMoving { get => _isMoving; set { _isMoving = value; } }
    private bool _isMoving = false;

    public Border GetBorder()
    {
        return borderA;
    }

    private void Update()
    {
        Border.CheckAndResolve_BorderOutOfBorder(borderB, borderA);
    }

    private void OnDrawGizmos()
    {
        if (!borderA) return;
        if (!borderB) return;

        Gizmos.color = Color.white;

        Rect rectA = (Rect)borderA;
        Rect rectB = (Rect)borderB;

        //DrawRect(rectA, Color.red);
        //DrawRect(rectB, Color.blue);

        Vector2 dir = borderA.transform.parent.position - borderB.transform.parent.position;
        Ray2D ray = new Ray2D(borderA.transform.parent.position, dir);

        Vector2 hitPoint;
        if (RayExtention.GetIntersectionOfRayInRect(in ray, in rectB, out hitPoint))
        {
            Gizmos.DrawSphere(new Vector3(hitPoint.x, hitPoint.y, 0), 0.1f);
            Debug.Log(hitPoint);
        }

        Gizmos.DrawRay(borderB.transform.parent.position, (borderA.transform.parent.position - borderB.transform.parent.position)*100);
    }

    private void DrawRect(Rect rect , Color color)
    {
        Gizmos.color = Color.black;
        Vector2 upLeft = rect.position;
        Vector2 upRight = new Vector2(rect.x + rect.width, rect.y);
        Vector2 downLeft = new Vector2(rect.x, rect.y - rect.height);
        Vector2 downRight = new Vector2(rect.x + rect.width, rect.y - rect.height);

        Gizmos.DrawLine(upLeft, upRight);
        Gizmos.DrawLine(downLeft, downRight);
        Gizmos.DrawLine(upRight, downRight);
        Gizmos.DrawLine(upLeft, downLeft);

        Gizmos.DrawLine(downLeft, upRight);
        Gizmos.DrawLine(upLeft, downRight);
    }
}
