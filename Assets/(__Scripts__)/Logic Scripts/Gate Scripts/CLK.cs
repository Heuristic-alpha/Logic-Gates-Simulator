using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CLK : MonoBehaviour, IBorderable, IClickableVirutalButton, IColorable, IItemable
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject _clkTextGameObject;

    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] SpriteRenderer _virutalButtonSpriteRenderer;
    private TextMeshProUGUI _clk_text;
    private PinManager _pinManager;

    // C# Properties: //////////////////////////////////////////////////////////
    public LogicColor LogicColor
    {
        get { return _logicColor; }
        set { _logicColor = value; }
    }

    public bool IsMoving { get => _isMoving; set { _isMoving = value; } }


    // C# Fields: //////////////////////////////////////////////////////////////
    private const float _1HZ_PERIUD = 1;
    private const float _2HZ_PERIUD = (float)1 / 2;
    private const float _4HZ_PERIUD = (float)1 / 4;
    private const float _8HZ_PERIUD = (float)1 / 8;
    private const float _16HZ_PERIUD = (float)1 / 16;
    private const float _32HZ_PERIUD = (float)1 / 32;

    [SerializeField] Color clk_onColor;
    [SerializeField] Color clk_offColor;
    [SerializeField] Color _onColor;
    [SerializeField] Color _offColor;
    private LogicColor _logicColor;
    
    private Border _border;
    private bool _isMoving = false;

    [SerializeField] Item _item;
    private Item_Rotation _item_Rotation = Item_Rotation.n;

    private LogicState _currentLogicState = LogicState.Off;
    private Clk_state _currentClkState = Clk_state.off;
    private float _lastPulseTime;
    private float _timeBetweenEachPulse;

    private IWirable _outputPin_Iwireable;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _border = GetComponentInChildren<Border>();
        _pinManager = GetComponent<PinManager>();
        _logicColor = new LogicColor(_onColor, _offColor);
        _clk_text = _clkTextGameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _outputPin_Iwireable = _pinManager.GetOutputPin_IWirable(0);
        _outputPin_Iwireable.LogicColor = LogicColor;
        UpdateCLKVisual();
    }

    private void Update()
    {     
        if (_currentClkState == Clk_state.off)
        {
            SetOutputPin(LogicState.Off);
        }
        else
        {
            if((Time.time - _lastPulseTime) > _timeBetweenEachPulse)
            {
                Pulse();
            }
        }
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public Border GetBorder() => _border;
    public PinManager GetPinManager() => _pinManager;
    public void ClickVirutalButton()
    {
        ChangeState();
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
        ResetWorldRotationOfClkText();
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
        ResetWorldRotationOfClkText();
    }
    public void RotateLeft()
    {
        if (_item_Rotation == Item_Rotation.n) _item_Rotation = Item_Rotation.w;
        else if (_item_Rotation == Item_Rotation.w) _item_Rotation = Item_Rotation.s;
        else if (_item_Rotation == Item_Rotation.s) _item_Rotation = Item_Rotation.e;
        else if (_item_Rotation == Item_Rotation.e) _item_Rotation = Item_Rotation.n;

        transform.Rotate(Vector3.forward, +90, Space.World);
        _border.RotatePoints_Plus90();
        ResetWorldRotationOfClkText();
    }


    // C# Private Methods: /////////////////////////////////////////////////////
    private void UpdateCLKVisual()
    {
        if (_currentClkState == Clk_state.off) _virutalButtonSpriteRenderer.color = clk_offColor;
        else  _virutalButtonSpriteRenderer.color = clk_onColor;
    }
    private void SetState(Clk_state state)
    {
        _currentClkState = state;
        switch (state)
        {
            case Clk_state.off:
                _clk_text.text = "<b>clk</b>\noff";
                break;

            case Clk_state._1_hz:
                _timeBetweenEachPulse = _1HZ_PERIUD;
                _clk_text.text = "<b>clk</b>\n1 hz";
                break;

            case Clk_state._2_hz:
                _timeBetweenEachPulse = _2HZ_PERIUD;
                _clk_text.text = "<b>clk</b>\n2 hz";
                break;

            case Clk_state._4_hz:
                _timeBetweenEachPulse = _4HZ_PERIUD;
                _clk_text.text = "<b>clk</b>\n4 hz";
                break;

            case Clk_state._8_hz:
                _timeBetweenEachPulse = _8HZ_PERIUD;
                _clk_text.text = "<b>clk</b>\n8 hz";
                break;

            case Clk_state._16_hz:
                _timeBetweenEachPulse = _16HZ_PERIUD;
                _clk_text.text = "<b>clk</b>\n16 hz";
                break;
            case Clk_state._32_hz:
                _timeBetweenEachPulse = _32HZ_PERIUD;
                _clk_text.text = "<b>clk</b>\n32 hz";
                break;
            default:
                break;
        }
    }
    private void ChangeState()
    {
        switch (_currentClkState)
        {
            case Clk_state.off:
                SetState(Clk_state._1_hz);               

                break;
            case Clk_state._1_hz:
                SetState(Clk_state._2_hz);

                break;
            case Clk_state._2_hz:
                SetState(Clk_state._4_hz);

                break;
            case Clk_state._4_hz:
                SetState(Clk_state._8_hz);

                break;
            case Clk_state._8_hz:
                SetState(Clk_state._16_hz);

                break;
            case Clk_state._16_hz:
                SetState(Clk_state._32_hz);

                break;
            case Clk_state._32_hz:
                SetState(Clk_state.off);

                break;
            default:
                throw new System.Exception("Error");               
        }
        Pulse();
        UpdateCLKVisual();
    }
    private void SetOutputPin(LogicState logicState)
    {
        _outputPin_Iwireable.LogicState = logicState;
        _outputPin_Iwireable.UpdateVisual();
    }
    private void InvertCurrentLogicState()
    {
        if(_currentLogicState == LogicState.On) _currentLogicState = LogicState.Off;
        else if (_currentLogicState == LogicState.Off) _currentLogicState = LogicState.On;
    }
    private void Pulse()
    {
        _lastPulseTime = Time.time;
        InvertCurrentLogicState();
        SetOutputPin(_currentLogicState);
    }
    private void ResetWorldRotationOfClkText()
    {
        _clkTextGameObject.transform.rotation = Quaternion.identity;
    }


    // others: ////////////////////////////////////////////////////////////////
    private enum Clk_state : byte
    {
        off = 0,
        _1_hz = 1,
        _2_hz = 2,
        _4_hz = 4,
        _8_hz = 8,
        _16_hz = 16,
        _32_hz = 32,
    }

} // end of class