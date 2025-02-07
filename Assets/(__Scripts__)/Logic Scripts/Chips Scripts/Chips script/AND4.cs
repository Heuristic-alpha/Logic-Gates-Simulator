using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AND4 : ChipBase
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
        IWirable inputPin_wirable_D = _pinManager.GetInputPin_IWirable(3);

        IWirable outputPin_wirable = _pinManager.GetOutputPin_IWirable(0);

        // get LogicState of all inputPins to update visual of all of them:
        LogicState inputState_A = inputPin_wirable_A.LogicState;
        LogicState inputState_B = inputPin_wirable_B.LogicState;
        LogicState inputState_C = inputPin_wirable_C.LogicState;
        LogicState inputState_D = inputPin_wirable_D.LogicState;

        if (inputState_A == LogicState.On &&
            inputState_B == LogicState.On &&
            inputState_C == LogicState.On &&
            inputState_D == LogicState.On )
        {
            outputPin_wirable.LogicState = LogicState.On;
        }
        else
        {
            outputPin_wirable.LogicState = LogicState.Off;
        }

    }

    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class