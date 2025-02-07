using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoder_2x4 : ChipBase
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

        if (inputPin_wirable_B.LogicState == LogicState.Off)
        {
            if (inputPin_wirable_A.LogicState == LogicState.Off)
            {
                SetOnlyOneLogicState(0);
            }
            else if (inputPin_wirable_A.LogicState == LogicState.On)
            {
                SetOnlyOneLogicState(1);
            }
        }
        else if (inputPin_wirable_B.LogicState == LogicState.On)
        {
            if (inputPin_wirable_A.LogicState == LogicState.Off)
            {
                SetOnlyOneLogicState(2);
            }
            else if (inputPin_wirable_A.LogicState == LogicState.On)
            {
                SetOnlyOneLogicState(3);
            }
        }
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void SetOnlyOneLogicState(int index)
    {
        int length = _pinManager.OutputPinsLength;
        for (int i = 0; i < length; ++i)
        {
            IWirable wirable = _pinManager.GetOutputPin_IWirable(i);
            if (i == index)
            {
                wirable.LogicState = LogicState.On;
            }
            else
            {
                wirable.LogicState = LogicState.Off;
            }
        }
    }

} // end of class