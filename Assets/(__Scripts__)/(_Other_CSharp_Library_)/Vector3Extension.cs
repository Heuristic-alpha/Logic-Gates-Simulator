using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 Parse(string content)
    {
        string[] segments = content.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        float x = float.Parse(segments[0]);
        float y = float.Parse(segments[1]);
        float z = float.Parse(segments[2]);

        return new Vector3(x, y, z);
    }
    public static string ToString(Vector3 vector3)
    {
        return $"{vector3.x} {vector3.y} {vector3.z}";
    }


}
