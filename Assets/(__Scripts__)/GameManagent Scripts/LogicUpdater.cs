using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicUpdater : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    // C# Properties: //////////////////////////////////////////////////////////
    public static LogicUpdater Singeleton { get { return _singeleton; } }

    // C# Fields: //////////////////////////////////////////////////////////////
    private static LogicUpdater _singeleton;
    private List<ILogicHandler> _logicHandlers = new List<ILogicHandler>();
    private List<Wire> _wires = new List<Wire>();

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        if (_singeleton != null) Destroy(this);
        else _singeleton = this;
    }

    private void FixedUpdate()
    {
        foreach (ILogicHandler handler in _logicHandlers)
        {
            handler.HandleLogic();
            handler.GetPinManager().UpdateAllPinsVisual();
        }
        foreach (Wire wire in _wires)
        {
            wire.TransferLogicAndColor();
        }
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////

    public void AddLogicHandler(ILogicHandler handler)
    {
        _logicHandlers.Add(handler);
    }
    public void RemoveLogicHandler(ILogicHandler handler)
    {
        _logicHandlers.Remove(handler);
    }
    public void AddWire(Wire wire) 
    {  
        _wires.Add(wire);
    }
    public void RemoveWire(Wire wire)
    {
        _wires.Remove(wire);
    }
    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class