using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGate : MonoBehaviour, IBorderable, IClickableVirutalButton, IColorable, IItemable, ILogicHandler
{   
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] SpriteRenderer _virutalButtonSpriteRenderer;
    private PinManager _pinManager;

    // C# Properties: //////////////////////////////////////////////////////////
    public LogicColor LogicColor
    {
        get { return _logicColor; }
        set { _logicColor = value; }
    }

    public bool IsMoving { get => _isMoving; set { _isMoving = value; } }
    

    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] Color _onColor;
    [SerializeField] Color _offColor;
    private LogicColor _logicColor;

    private LogicState _currentLogicState = LogicState.Off;
    private Border _border;
    private bool _isMoving = false;

    [SerializeField] Item _item;
    private Item_Rotation _item_Rotation = Item_Rotation.n;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _border = GetComponentInChildren<Border>();
        _pinManager = GetComponent<PinManager>();
        _logicColor = new LogicColor(_onColor, _offColor);
    }

    private void Start()
    {
        _pinManager.GetOutputPin_IWirable(0).LogicColor = LogicColor;
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
    public PinManager GetPinManager() => _pinManager;
    public void ClickVirutalButton()
    {
        // click for turn on:
        if (_currentLogicState == LogicState.Off)
        {
            _currentLogicState = LogicState.On;          
        }
        // click for turn off:
        else if(_currentLogicState == LogicState.On)
        {
            _currentLogicState = LogicState.Off;           
        }
    }   
    public Item GetItem() => _item;
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

    public void HandleLogic()
    {
        IWirable outputPin = _pinManager.GetOutputPin_IWirable(0);
        outputPin.LogicState = _currentLogicState;
        outputPin.LogicColor = _logicColor;

        SetColor();
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void SetColor()
    {
        if (_currentLogicState == LogicState.On) _virutalButtonSpriteRenderer.color = _logicColor.OnColor;
        else if (_currentLogicState == LogicState.Off) _virutalButtonSpriteRenderer.color = _logicColor.OffColor;
    }

} // end of class