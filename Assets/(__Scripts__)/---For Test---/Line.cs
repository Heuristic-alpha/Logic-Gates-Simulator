using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] Transform t1;
    [SerializeField] Transform t2;

    [SerializeField] bool canRotate = false;
    [SerializeField] bool followMouse = false;
    [SerializeField] float rotateSpeed = 10;
    [SerializeField] Color color;

    public Vector2 p1 { get { return t1.position; } }
    public Vector2 p2 { get { return t2.position; } }

    private void Update()
    {
        if(canRotate)
        {
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        }

        if (followMouse)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); mouseWorldPos.z = 0;
            transform.position = mouseWorldPos;
        }
    }

    private void OnDrawGizmos()
    {
        if (!t1 || !t2) return;

        Gizmos.color = color;
        Gizmos.DrawLine(t1.position, t2.position);
    }
}
