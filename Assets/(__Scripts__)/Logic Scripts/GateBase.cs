using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class of all LogicGates
/// </summary>
public abstract class GateBase : MonoBehaviour, IBorderable, IColorable, IItemable, ILogicHandler
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    protected Border _border;
    protected PinManager _pinManager;

    // C# Properties: //////////////////////////////////////////////////////////
    public LogicColor LogicColor
    {
        get { return _logicColor; }
        set { _logicColor = value; }
    }
    public bool IsMoving { get => _isMoving; set { _isMoving = value; } }


    // C# Fields: //////////////////////////////////////////////////////////////  
    [SerializeField] protected Color _onColor;
    [SerializeField] protected Color _offColor;
    [SerializeField] protected Item _item;
    protected LogicColor _logicColor;
    protected Item_Rotation _item_Rotation = Item_Rotation.n;
    protected bool _isMoving = false;

    // Unity Main Events: //////////////////////////////////////////////////////
    protected virtual void Awake()
    {
        _border = GetComponentInChildren<Border>();
        _pinManager = GetComponent<PinManager>();
        _logicColor = new LogicColor(_onColor, _offColor);
    }

    protected virtual void Start()
    {
        InitOutputPinsColor();
    }


    // Unity Other Events: /////////////////////////////////////////////////////
    protected virtual void OnEnable()
    {
        LogicUpdater.Singeleton.AddLogicHandler(this);
    }
    protected virtual void OnDisable()
    {
        LogicUpdater.Singeleton.RemoveLogicHandler(this);
    }

    // C# Public Methods: //////////////////////////////////////////////////////
    public Border GetBorder() => _border;
    public Item GetItem() => _item;
    public PinManager GetPinManager() => _pinManager;
    public virtual void SetItem_Rotation(Item_Rotation new_item_rotation)
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
    public virtual void RotateRight()
    {
        if (_item_Rotation == Item_Rotation.n) _item_Rotation = Item_Rotation.e;
        else if (_item_Rotation == Item_Rotation.e) _item_Rotation = Item_Rotation.s;
        else if (_item_Rotation == Item_Rotation.s) _item_Rotation = Item_Rotation.w;
        else if (_item_Rotation == Item_Rotation.w) _item_Rotation = Item_Rotation.n;

        transform.Rotate(Vector3.forward, -90, Space.World);
        _border.RotatePoints_Minus90();

    }
    public virtual void RotateLeft()
    {
        if (_item_Rotation == Item_Rotation.n) _item_Rotation = Item_Rotation.w;
        else if (_item_Rotation == Item_Rotation.w) _item_Rotation = Item_Rotation.s;
        else if (_item_Rotation == Item_Rotation.s) _item_Rotation = Item_Rotation.e;
        else if (_item_Rotation == Item_Rotation.e) _item_Rotation = Item_Rotation.n;

        transform.Rotate(Vector3.forward, +90, Space.World);
        _border.RotatePoints_Plus90();
    }
    public abstract void HandleLogic();

    // C# Private Methods: /////////////////////////////////////////////////////
    // C# Protected Methods: /////////////////////////////////////////////////////
    protected void InitOutputPinsColor()
    {
        IWirable[] wirables = _pinManager.GetOutputPin_IWirables();
        foreach (IWirable wirable in wirables)
        {
            wirable.LogicColor = _logicColor;
        }
    }
      
} // end of class