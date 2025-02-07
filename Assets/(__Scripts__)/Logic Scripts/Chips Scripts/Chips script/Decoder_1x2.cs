using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoder_1x2: ChipBase
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
        IWirable inputPin_wirable = _pinManager.GetInputPin_IWirable(0);       
       
        if (inputPin_wirable.LogicState == LogicState.Off)
        {
            SetOnlyOneLogicState(0);
        }
        else if (inputPin_wirable.LogicState == LogicState.On)
        {
            SetOnlyOneLogicState(1);
        }
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void SetOnlyOneLogicState(int index)
    {
        int length = _pinManager.OutputPinsLength;
        for(int i = 0; i < length; ++i)
        {
            IWirable wirable = _pinManager.GetOutputPin_IWirable(i);
            if(i == index)
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