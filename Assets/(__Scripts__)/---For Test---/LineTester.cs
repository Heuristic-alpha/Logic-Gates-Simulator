using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTester : MonoBehaviour
{
    [SerializeField] Line staticLine;
    [SerializeField] Line dynamicLine;

    private void OnDrawGizmos()
    {
        if(!staticLine || !dynamicLine) return;

        Vector2 dir = dynamicLine.p2 - dynamicLine.p1;
        Ray2D ray = new Ray2D(dynamicLine.transform.position, dir);
        LineSegment lineSegment = new LineSegment(staticLine.p1, staticLine.p2);

        // draw ray:
        Gizmos.color = Color.green;
        Gizmos.DrawRay(ray.origin, ray.direction);

        Vector2 hitPos;
        if (RayExtention.GetIntersectionOfRayToLine(in ray, in lineSegment, out hitPos))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitPos, 0.1f);
        }
    }

}
