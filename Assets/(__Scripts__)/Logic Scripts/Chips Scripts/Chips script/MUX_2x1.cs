using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MUX_2x1 : ChipBase
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
        IWirable inputPin_wirable_A = _pinManager.GetInputPin_IWirable(0);
        IWirable inputPin_wirable_B = _pinManager.GetInputPin_IWirable(1);

        IWirable inputPin_wirable_C = _pinManager.GetInputPin_IWirable(2);
      
        IWirable outputPin_wirable = _pinManager.GetOutputPin_IWirable(0);

        // get LogicState of all inputPins to update visual of all of them:
        LogicState inputState_A = inputPin_wirable_A.LogicState;
        LogicState inputState_B = inputPin_wirable_B.LogicState;
        LogicState inputState_C = inputPin_wirable_C.LogicState;

        if (inputPin_wirable_C.LogicState == LogicState.Off)
        {
            outputPin_wirable.LogicColor = inputPin_wirable_A.LogicColor;
            outputPin_wirable.LogicState = inputPin_wirable_A.LogicState;
        }
        else if (inputPin_wirable_C.LogicState == LogicState.On)
        {
            outputPin_wirable.LogicColor = inputPin_wirable_B.LogicColor;
            outputPin_wirable.LogicState = inputPin_wirable_B.LogicState;
        }
    }

    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class