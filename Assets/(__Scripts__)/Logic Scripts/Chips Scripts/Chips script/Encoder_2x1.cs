using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encoder_2x1 : ChipBase
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

        IWirable outputPin_wirable = _pinManager.GetOutputPin_IWirable(0);

        // get LogicState of all inputPins to update visual of all of them:
        LogicState inputState_A = inputPin_wirable_A.LogicState;
        LogicState inputState_B = inputPin_wirable_B.LogicState;       

        if (inputPin_wirable_B.LogicState == LogicState.On)
        {
            outputPin_wirable.LogicState = LogicState.On;
        }
        else if (inputPin_wirable_A.LogicState == LogicState.On)
        {
            outputPin_wirable.LogicState = LogicState.Off;
        }
        else // input error
        {
            outputPin_wirable.LogicState = LogicState.Off;
        }
    }

    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class