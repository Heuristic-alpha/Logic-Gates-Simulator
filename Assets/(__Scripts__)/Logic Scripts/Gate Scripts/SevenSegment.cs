using UnityEngine;

public class SevenSegment : MonoBehaviour, IBorderable, IItemable, ILogicHandler
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [Header("SpriteRenderers:")]
    [SerializeField] SpriteRenderer _srA;
    [SerializeField] SpriteRenderer _srB;
    [SerializeField] SpriteRenderer _srC;
    [SerializeField] SpriteRenderer _srD;
    [SerializeField] SpriteRenderer _srE;
    [SerializeField] SpriteRenderer _srF;
    [SerializeField] SpriteRenderer _srG;

    private Border _border;
    private PinManager _pinManager;

    // C# Properties: //////////////////////////////////////////////////////////
    public bool IsMoving { get => _isMoving; set { _isMoving = value; } }
    

    // C# Fields: //////////////////////////////////////////////////////////////
    [Header("Set Colors:")]
    [SerializeField] Color _onColor;
    [SerializeField] Color _offColor;

    private LogicColor _logicColor;

    [SerializeField] Item _item;
    private Item_Rotation _item_Rotation = Item_Rotation.n;
    private bool _isMoving = false;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _logicColor = new LogicColor(_onColor, _offColor);
        _border = GetComponentInChildren<Border>();
        _pinManager = GetComponent<PinManager>();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    private void OnEnable()
    {
        LogicUpdater.Singeleton.AddLogicHandler(this);
    }
    private void OnDisable()
    {
        LogicUpdater.Singeleton.RemoveLogicHandler(this);
    }

    // C# Public Methods: //////////////////////////////////////////////////////
    public Border GetBorder() => _border;
    public Item GetItem() => _item;
    public PinManager GetPinManager() => _pinManager;
    public void SetItem_Rotation(Item_Rotation new_item_rotation)
    {
        if (_item_Rotation == Item_Rotation.n)
        {
            if (new_item_rotation == Item_Rotation.n) { }// do nothing
            else if (new_item_rotation == Item_Rotation.e)
            {
                _border.RotatePoints_Minus90();
                transform.Rotate(Vector3.forward, -90, Space.World);
                _item_Rotation = new_item_rotation;
            }
            else if (new_item_rotation == Item_Rotation.s)
            {
                _border.RotatePoints_Plus90();
                _border.RotatePoints_Plus90();
                transform.Rotate(Vector3.forward, 180, Space.World);
                _item_Rotation = new_item_rotation;
            }
            else if (new_item_rotation == Item_Rotation.w)
            {
                _border.RotatePoints_Plus90();
                transform.Rotate(Vector3.forward, +90, Space.World);
                _item_Rotation = new_item_rotation;
            }
        }
        else if (_item_Rotation == Item_Rotation.e)
        {
            if (new_item_rotation == Item_Rotation.e) { }// do nothing
            else if (new_item_rotation == Item_Rotation.s)
            {
                _border.RotatePoints_Minus90();
                transform.Rotate(Vector3.forward, -90, Space.World);
                _item_Rotation = new_item_rotation;
            }
            else if (new_item_rotation == Item_Rotation.w)
            {
                _border.RotatePoints_Plus90();
                _border.RotatePoints_Plus90();
                transform.Rotate(Vector3.forward, 180, Space.World);
                _item_Rotation = new_item_rotation;
            }
            else if (new_item_rotation == Item_Rotation.n)
            {
                _border.RotatePoints_Plus90();
                transform.Rotate(Vector3.forward, +90, Space.World);
                _item_Rotation = new_item_rotation;
            }
        }
        else if (_item_Rotation == Item_Rotation.s)
        {
            if (new_item_rotation == Item_Rotation.s) { }// do nothing
            else if (new_item_rotation == Item_Rotation.w)
            {
                _border.RotatePoints_Minus90();
                transform.Rotate(Vector3.forward, -90, Space.World);
                _item_Rotation = new_item_rotation;
            }
            else if (new_item_rotation == Item_Rotation.n)
            {
                _border.RotatePoints_Plus90();
                _border.RotatePoints_Plus90();
                transform.Rotate(Vector3.forward, 180, Space.World);
                _item_Rotation = new_item_rotation;
            }
            else if (new_item_rotation == Item_Rotation.e)
            {
                _border.RotatePoints_Plus90();
                transform.Rotate(Vector3.forward, +90, Space.World);
                _item_Rotation = new_item_rotation;
            }
        }
        else if (_item_Rotation == Item_Rotation.w)
        {
            if (new_item_rotation == Item_Rotation.w) { }// do nothing
            else if (new_item_rotation == Item_Rotation.n)
            {
                _border.RotatePoints_Minus90();
                transform.Rotate(Vector3.forward, -90, Space.World);
                _item_Rotation = new_item_rotation;
            }
            else if (new_item_rotation == Item_Rotation.e)
            {
                _border.RotatePoints_Plus90();
                _border.RotatePoints_Plus90();
                transform.Rotate(Vector3.forward, 180, Space.World);
                _item_Rotation = new_item_rotation;
            }
            else if (new_item_rotation == Item_Rotation.s)
            {
                _border.RotatePoints_Plus90();
                transform.Rotate(Vector3.forward, +90, Space.World);
                _item_Rotation = new_item_rotation;
            }
        }
    }
    public Item_Rotation GetItem_Rotation() => _item_Rotation;
    public void RotateRight()
    {
        if (_item_Rotation == Item_Rotation.n) _item_Rotation = Item_Rotation.e;
        else if (_item_Rotation == Item_Rotation.e) _item_Rotation = Item_Rotation.s;
        else if (_item_Rotation == Item_Rotation.s) _item_Rotation = Item_Rotation.w;
        else if (_item_Rotation == Item_Rotation.w) _item_Rotation = Item_Rotation.n;

        transform.Rotate(Vector3.forward, -90, Space.World);
        _border.RotatePoints_Minus90();
    }
    public void RotateLeft()
    {
        if (_item_Rotation == Item_Rotation.n) _item_Rotation = Item_Rotation.w;
        else if (_item_Rotation == Item_Rotation.w) _item_Rotation = Item_Rotation.s;
        else if (_item_Rotation == Item_Rotation.s) _item_Rotation = Item_Rotation.e;
        else if (_item_Rotation == Item_Rotation.e) _item_Rotation = Item_Rotation.n;

        transform.Rotate(Vector3.forward, +90, Space.World);
        _border.RotatePoints_Plus90();
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void SevenSegmentDisplay()
    {
        IWirable inputPinA = _pinManager.GetInputPin_IWirable(0);
        IWirable inputPinB = _pinManager.GetInputPin_IWirable(1);
        IWirable inputPinC = _pinManager.GetInputPin_IWirable(2);
        IWirable inputPinD = _pinManager.GetInputPin_IWirable(3);
        IWirable inputPinE = _pinManager.GetInputPin_IWirable(4);
        IWirable inputPinF = _pinManager.GetInputPin_IWirable(5);
        IWirable inputPinG = _pinManager.GetInputPin_IWirable(6);

        LogicState stateA = inputPinA.LogicState;
        LogicState stateB = inputPinB.LogicState;
        LogicState stateC = inputPinC.LogicState;
        LogicState stateD = inputPinD.LogicState;
        LogicState stateE = inputPinE.LogicState;
        LogicState stateF = inputPinF.LogicState;
        LogicState stateG = inputPinG.LogicState;        

        if (stateA == LogicState.On) _srA.color = _logicColor.OnColor;
        else _srA.color = _logicColor.OffColor;

        if (stateB == LogicState.On) _srB.color = _logicColor.OnColor;
        else _srB.color = _logicColor.OffColor;

        if (stateC == LogicState.On) _srC.color = _logicColor.OnColor;
        else _srC.color = _logicColor.OffColor;

        if (stateD == LogicState.On) _srD.color = _logicColor.OnColor;
        else _srD.color = _logicColor.OffColor;

        if (stateE == LogicState.On) _srE.color = _logicColor.OnColor;
        else _srE.color = _logicColor.OffColor;

        if (stateF == LogicState.On) _srF.color = _logicColor.OnColor;
        else _srF.color = _logicColor.OffColor;

        if (stateG == LogicState.On) _srG.color = _logicColor.OnColor;
        else _srG.color = _logicColor.OffColor;
    }
    public void HandleLogic()
    {
        SevenSegmentDisplay();
    }

} // end of class