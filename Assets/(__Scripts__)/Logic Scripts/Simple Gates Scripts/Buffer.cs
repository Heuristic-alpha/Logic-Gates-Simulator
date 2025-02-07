using UnityEngine;

public class Buffer : MonoBehaviour, IBorderable, IItemable, ILogicHandler
{
    // Unity GameObjects: //////////////////////////////////////////////////////   
    // Unity Components: ///////////////////////////////////////////////////////
    private Border _border;
    private PinManager _pinManager;

    // C# Properties: //////////////////////////////////////////////////////////
    public bool IsMoving { get => _isMoving; set { _isMoving = value; } }
    

    // C# Fields: //////////////////////////////////////////////////////////////  
    [SerializeField] Item _item;
    private Item_Rotation _item_Rotation = Item_Rotation.n;
    private bool _isMoving = false;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
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
    public void HandleLogic()
    {
        int outputPinsCount = _pinManager.OutputPinsLength;
        IWirable inputPin = _pinManager.GetInputPin_IWirable(0);
        LogicState inputPinState = inputPin.LogicState;
        LogicColor inputPinLogicColor = inputPin.LogicColor;     
        for (int i = 0; i < outputPinsCount; i++)
        {
            IWirable outputPin = _pinManager.GetOutputPin_IWirable(i);
            outputPin.LogicState = inputPinState;
            outputPin.LogicColor = inputPinLogicColor;        
        }
        
    }

    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class