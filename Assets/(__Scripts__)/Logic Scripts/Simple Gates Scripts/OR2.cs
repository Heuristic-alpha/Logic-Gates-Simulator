using UnityEngine;

public class OR2 : GateBase
{
    // Unity GameObjects: //////////////////////////////////////////////////////   
    // Unity Components: ///////////////////////////////////////////////////////
    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////   
    // Unity Main Events: //////////////////////////////////////////////////////
    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public override void HandleLogic()
    {
        IWirable inputPinA = _pinManager.GetInputPin_IWirable(0);
        IWirable inputPinB = _pinManager.GetInputPin_IWirable(1);
        IWirable outputPin = _pinManager.GetOutputPin_IWirable(0);
        LogicState inStateA = inputPinA.LogicState;
        LogicState inStateB = inputPinB.LogicState;

        outputPin.LogicState = LogicHelper.OR2(inStateA, inStateB);
    }

    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class