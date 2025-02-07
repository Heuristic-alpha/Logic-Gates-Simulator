using System;
using UnityEngine;
using T_Generator;

public class Wire : MonoBehaviour, IColorable
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    private GameObject _startPoint; // most be InputPin (Pin In)
    private GameObject _endPoint;   // most be OutputPin (Pin Out)
    public GameObject StartPoint
    {
        get { return _startPoint; }
        set
        {
            _startPoint = value;         
            _startPoint_Wirable = _startPoint.GetComponent<IWirable>();
        }
    }
    public GameObject EndPoint
    {
        get { return _endPoint; }
        set
        {
            _endPoint = value;
            _endPoint_Wirable = _endPoint.GetComponent<IWirable>();
        }
    }

    public LogicColor LogicColor 
    { 
        get => _logicColor;
        set => _logicColor = value;
    }

    // Unity Components: ///////////////////////////////////////////////////////
    private IWirable _startPoint_Wirable = null;
    private IWirable _endPoint_Wirable = null;

    private LineRenderer _lineRenderer;

    // C# Properties: ////////////////////////////////////////////////////////// 
    // C# Consts: //////////////////////////////////////////////////////////////
    public const float UnitLength = 0.6f;
    public const int MinimumPoint = 20;
    public const float WirePointsMinimumDistance = 0;
    public const float WirePointsMaximumDistance = 21;
    public const float HelperPointMinimumLength = 0.5f;
    public const float HelperPointMaximumLength = 3.5f;

    public const float DisToLenRatio = (WirePointsMaximumDistance - WirePointsMinimumDistance) / (HelperPointMaximumLength - HelperPointMinimumLength);

    // C# Fields: //////////////////////////////////////////////////////////////
    private LogicColor _logicColor;

    int lineResolution;
    float wireHelperPointLength;

    Vector2 _lastStartPointPosition;
    Vector2 _lastEndPointPosition;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // Check if startPoint and endPoint are already Exist:
        if (!_startPoint || !_endPoint) { SelfDestroy(); return; }

        // Update if WirePoints has change position:
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
    }
    // Unity Other Events: /////////////////////////////////////////////////////

    private void OnEnable()
    {
        LogicUpdater.Singeleton.AddWire(this);
    }
    private void OnDisable()
    {
        LogicUpdater.Singeleton.RemoveWire(this);
    }

    // C# Public Methods: //////////////////////////////////////////////////////
    public void TransferLogicAndColor() // Most be called from LogicUpdater
    {
        LogicState startPointState = _startPoint_Wirable.LogicState;
        LogicColor startPointColor = _startPoint_Wirable.LogicColor;
        _logicColor = startPointColor;
        _endPoint_Wirable.LogicState = startPointState;
        _endPoint_Wirable.LogicColor = startPointColor;

        // update wire color
        if (startPointState == LogicState.On) SetWireColor(startPointColor.OnColor);
        else if (startPointState == LogicState.Off) SetWireColor(startPointColor.OffColor);
        else
        {
            Debug.LogError("this wire startPoint LogicState is Set to None!");
        }
    }
    
    public WireInfo ToWireInfo(GameObject[] spawnedItems)
    {
        GameObject startPointItemGameObject = _startPoint_Wirable.GetItemGameObject();
        int localItemId_s = IndexOfWirePointsInArrayOf(spawnedItems, startPointItemGameObject);
        Trait pinType_s = _startPoint_Wirable.GetPinType();
        byte pinIndex_s = (byte)_startPoint_Wirable.GetPinIndex();

        GameObject endPointItemGameObject = _endPoint_Wirable.GetItemGameObject();
        int localItemId_e = IndexOfWirePointsInArrayOf(spawnedItems, endPointItemGameObject);
        Trait pinType_e = _endPoint_Wirable.GetPinType();
        byte pinIndex_e = (byte)_endPoint_Wirable.GetPinIndex();

        return new WireInfo(localItemId_s, pinType_s, pinIndex_s, localItemId_e, pinType_e, pinIndex_e);

        // local method:
        int IndexOfWirePointsInArrayOf(GameObject[] spawnedItems , GameObject wirePoint)
        {
            for(int i = 0; i < spawnedItems.Length; i++)
            {
                if (ReferenceEquals(wirePoint, spawnedItems[i])) return i;
            }
            return -1;
        }
}

    // C# Private Methods: /////////////////////////////////////////////////////
    private void UpdateLineRendererPoints()
    {
        CalculateLineSetting();
        _lineRenderer.positionCount = lineResolution;

        Vector3 helperPoint1 = _startPoint.transform.position + (_startPoint_Wirable.GetHelperPointDirection() * wireHelperPointLength);
        Vector3 helperPoint2 = _endPoint.transform.position + (_endPoint_Wirable.GetHelperPointDirection() * wireHelperPointLength);

        for (int i = 0; i < lineResolution; i++)
        {
            float currentT = T_Holder.Get_t(lineResolution, i);
            Vector3 currentPoint = Bezier.Cubic2D(_startPoint.transform.position,
                                                  helperPoint1,
                                                  helperPoint2,
                                                  _endPoint.transform.position, currentT);
            _lineRenderer.SetPosition(i, currentPoint);

        }
        
    }
    private void SetWireColor(in Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

    // Calculate lineResolution and wireHelperPointLength:
    private void CalculateLineSetting()
    {
        Vector2 distance = _startPoint.transform.position - _endPoint.transform.position;
        float distanceLength = Mathf.Abs(distance.magnitude);
        float clampedDistance = Mathf.Clamp(distanceLength, WirePointsMinimumDistance, WirePointsMaximumDistance);

        wireHelperPointLength = (clampedDistance / DisToLenRatio) + HelperPointMinimumLength;

        float temp = clampedDistance / UnitLength;
        temp += MinimumPoint;
        lineResolution = (int)temp;
    }
    private void UpdateWireIfWirePointsAreInit()
    {
        if (_startPoint == null || _endPoint == null) return;
        UpdateLineRendererPoints();
    }
    private void SelfDestroy()
    {
        if (_startPoint != null) _startPoint_Wirable.Wire_GameObject = null;
        if (_endPoint != null) _endPoint_Wirable.Wire_GameObject = null;

        Destroy(gameObject);
    }

} // end of class