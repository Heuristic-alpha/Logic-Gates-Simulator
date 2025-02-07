using System.Collections;
using System.Collections.Generic;
using T_Generator;
using UnityEngine;

public class PreviewWire : MonoBehaviour, IColorable
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject _startPoint;
    [SerializeField] GameObject _endPoint;

    // Unity Components: ///////////////////////////////////////////////////////
    private LineRenderer _lineRenderer;

    // C# Properties: //////////////////////////////////////////////////////////
    public LogicColor LogicColor
    {
        get { return _logicColor; }
        set { _logicColor = value; }
    }

    public bool IsPreviewWireStartFromStartPoint { get; set; }

    // C# Fields: //////////////////////////////////////////////////////////////
    private LogicColor _logicColor;   
    int lineResolution;
    float wireHelperPointLength;

    float t;
    int changeColorDirection = 1;
    [SerializeField] float changeColorSpeed = 1; 

    Vector2 _lastStartPointPosition;
    Vector2 _lastEndPointPosition;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
        t = 0;
        changeColorDirection = 1;
    }

    private void Update()
    {
        // Update lineRendererPoints if WirePoints has change:
        if ((_startPoint.transform.position.x != _lastStartPointPosition.x) || (_startPoint.transform.position.y != _lastStartPointPosition.y))
        {
            _lastStartPointPosition = _startPoint.transform.position;
            UpdateLineRendererPoints();
        }
        else if ((_endPoint.transform.position.x != _lastEndPointPosition.x) || (_endPoint.transform.position.y != _lastEndPointPosition.y))
        {
            _lastEndPointPosition = _endPoint.transform.position;
            UpdateLineRendererPoints();
        }

        // loop Wire color:
        LoopWireColor();
    }
    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    
    public void UpdateWire()
    {
        UpdateLineRendererPoints();
    }

    public void SetStartPoint_Position(Vector2 position)
    {
        _startPoint.transform.position = position;
    }
    public void SetStartPoint_Rotation(Quaternion rotation)
    {
        _startPoint.transform.rotation = rotation;
    }
    public void SetEndPoint_Position(Vector2 position)
    {
        _endPoint.transform.position = position;
    }
    public void SetEndPoint_Rotation(Quaternion rotation)
    {
        _endPoint.transform.rotation = rotation;
    }


    // C# Private Methods: /////////////////////////////////////////////////////
    private void UpdateLineRendererPoints()
    {
        CalculateLineSetting();
        _lineRenderer.positionCount = lineResolution;

        Vector3 helperPoint1 = _startPoint.transform.position + (_startPoint.transform.right * wireHelperPointLength);
        Vector3 helperPoint2 = _endPoint.transform.position + (-_endPoint.transform.right * wireHelperPointLength);

        for (int i = 0; i < lineResolution; i++)
        {
            float currentT = T_Holder.Get_t(lineResolution, i);
            Vector3 currentPoint = Bezier.Cubic2D(_startPoint.transform.position,
                                                  IsPreviewWireStartFromStartPoint == true ? helperPoint1 : _startPoint.transform.position,
                                                  IsPreviewWireStartFromStartPoint == true ? _endPoint.transform.position : helperPoint2,
                                                  _endPoint.transform.position, currentT) ;
            _lineRenderer.SetPosition(i, currentPoint);
        }
        
    }

    // Calculate lineResolution and wireHelperPointLength:
    private void CalculateLineSetting()
    {
        Vector2 distance = _startPoint.transform.position - _endPoint.transform.position;
        float distanceLength = Mathf.Abs(distance.magnitude);
        float clampedDistance = Mathf.Clamp(distanceLength, Wire.WirePointsMinimumDistance, Wire.WirePointsMaximumDistance);

        wireHelperPointLength = (clampedDistance / Wire.DisToLenRatio) + Wire.HelperPointMinimumLength;

        float temp = clampedDistance / Wire.UnitLength;
        temp += Wire.MinimumPoint;
        lineResolution = (int)temp;
    }

    private void LoopWireColor()
    {
        if (t > 1)
        {
            changeColorDirection *= -1;
            t = 1;
        }
        else if (t < 0)
        {
            changeColorDirection *= -1;
            t = 0;
        }


        float temp = Time.deltaTime * changeColorDirection * changeColorSpeed;
        t += temp;
        Color tempColor = Color.Lerp(_logicColor.OffColor, _logicColor.OnColor, t);
        _lineRenderer.startColor = tempColor;
        _lineRenderer.endColor = tempColor;
     
    }

} // end of class