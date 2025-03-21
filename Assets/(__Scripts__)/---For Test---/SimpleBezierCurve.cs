using System.Collections;
using UnityEngine;
using HSCL.Bezier;

public class SimpleBezierCurve : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] Transform t0;
    [SerializeField] Transform t1;
    [SerializeField] Transform t2;
    [SerializeField] Transform t3;
    private LineRenderer lr;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    const float unitLength = 2f;
    const int minPoint = 12;
    int lineResolution;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        GenerateLinePoints();
    }
    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    // C# Private Methods: /////////////////////////////////////////////////////
    private void GenerateLinePoints()
    {
        CalculateLineResolution();
        lr.positionCount = lineResolution;
        float step = (float)1 / (lineResolution - 1);
        int i;
        for (i = 0; i < lineResolution - 1; i++)
        {
            float currentT = i * step;
            Vector3 currentPoint = Bezier.Cubic2D(t0.position, t1.position, t2.position, t3.position, currentT);
            lr.SetPosition(i, currentPoint);
        }
        //for the last point:
        float lastT = i * step;
        Vector3 lastPoint = Bezier.Cubic2D(t0.position, t1.position, t2.position, t3.position, lastT);
        lr.SetPosition(i, lastPoint);
    }

    private void CalculateLineResolution()
    {
        Vector2 distance = t0.position - t3.position;
        float distanceLength = Mathf.Abs(distance.magnitude);

        float temp = distanceLength / unitLength;
        temp += minPoint;

        lineResolution = (int)temp;
    }

    private IEnumerator UpdateLine()
    {
        while (true)
        {
            yield return null;
            GenerateLinePoints();
        }
    }

} // end of class