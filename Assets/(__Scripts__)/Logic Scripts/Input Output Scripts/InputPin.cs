using UnityEngine;

public class InputPin : MonoBehaviour, IWirable
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    private GameObject _wire_GameObject; // we dont need this in InputPin class BUT wire class NEED it!
    public GameObject Wire_GameObject
    {
        get { return _wire_GameObject; }
        set { _wire_GameObject = value; }
    }
    [SerializeField] GameObject _itemGameObject;

    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] SpriteRenderer _pinSprite_SpriteRenderer;
    private TraitHolder _traitHolder;

    // C# Properties: //////////////////////////////////////////////////////////
    public LogicColor LogicColor
    {
        get 
        {
            if (_wire_GameObject == null) 
            {
                _logicColor = GameManager.Instance.LogicDefaultColor;
                return _logicColor;
            }
            else return _logicColor; 
        }
        set
        {
            _logicColor = value;        
        }
    }
    public LogicState LogicState
    {
        get 
        {
            if (_wire_GameObject == null) 
            { 
                _logicState = LogicState.Off;
                _logicColor = GameManager.Instance.LogicDefaultColor;
                return _logicState;
            }
            else return _logicState; 
        }
        set { _logicState = value; }
    }

    // C# Fields: //////////////////////////////////////////////////////////////
    private LogicColor _logicColor;
    private LogicState _logicState;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        Wire_GameObject = null;
        _logicState = LogicState.Off;
        _logicColor = GameManager.Instance.LogicDefaultColor;
        _traitHolder = GetComponent<TraitHolder>();
    }  

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public Vector3 GetHelperPointDirection() => -transform.right;
    public GameObject GetItemGameObject() => _itemGameObject;
    public Trait GetPinType()
    {
        if (_traitHolder.Contain(Trait.Input)) return Trait.Input;
        else if (_traitHolder.Contain(Trait.Output)) return Trait.Output;
        else return Trait.None; // most an Error accoured
    }
    public int GetPinIndex()
    {
        PinManager pm = _itemGameObject.GetComponent<PinManager>();
        Trait trait = GetPinType();
        if (trait == Trait.Input)
        {
            for (int i = 0; i < pm.InputPinsLength; i++)
            {
                if (ReferenceEquals(gameObject, pm.GetInputPin(i))) return i;
                continue;
            }
            return 1002;// Error
        }
        else if (trait == Trait.Output)
        {
            for (int i = 0; i < pm.OutputPinsLength; i++)
            {
                if (ReferenceEquals(gameObject, pm.GetOutputPin(i))) return i;
                continue;
            }
            return 1004;// Error
        }
        else return 1005;// Error
    }
    public void UpdateVisual()
    {
        if (_logicState == LogicState.On)
        {
            _pinSprite_SpriteRenderer.color = _logicColor.OnColor;
        }
        else if (_logicState == LogicState.Off)
        {
            _pinSprite_SpriteRenderer.color = _logicColor.OffColor;
        }
        else _pinSprite_SpriteRenderer.color = Color.white;
    }

    // C# Private Methods: /////////////////////////////////////////////////////


} // end of class