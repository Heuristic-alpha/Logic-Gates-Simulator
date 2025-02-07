using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MUX_8x1 : ChipBase
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
        IWirable inputPin_wirable_E = _pinManager.GetInputPin_IWirable(4);
        IWirable inputPin_wirable_F = _pinManager.GetInputPin_IWirable(5);
        IWirable inputPin_wirable_G = _pinManager.GetInputPin_IWirable(6);
        IWirable inputPin_wirable_H = _pinManager.GetInputPin_IWirable(7);

        IWirable inputPin_wirable_I = _pinManager.GetInputPin_IWirable(8);
        IWirable inputPin_wirable_J = _pinManager.GetInputPin_IWirable(9);
        IWirable inputPin_wirable_K = _pinManager.GetInputPin_IWirable(10);

        IWirable outputPin_wirable = _pinManager.GetOutputPin_IWirable(0);

        // get LogicState of all inputPins to update visual of all of them:
        LogicState inputState_A = inputPin_wirable_A.LogicState;
        LogicState inputState_B = inputPin_wirable_B.LogicState;
        LogicState inputState_C = inputPin_wirable_C.LogicState;
        LogicState inputState_D = inputPin_wirable_D.LogicState;
        LogicState inputState_E = inputPin_wirable_E.LogicState;
        LogicState inputState_F = inputPin_wirable_F.LogicState;
        LogicState inputState_G = inputPin_wirable_G.LogicState;
        LogicState inputState_H = inputPin_wirable_H.LogicState;

        if (inputPin_wirable_K.LogicState == LogicState.Off)
        {
            if (inputPin_wirable_J.LogicState == LogicState.Off)
            {
                if (inputPin_wirable_I.LogicState == LogicState.Off)
                {
                    outputPin_wirable.LogicColor = inputPin_wirable_A.LogicColor;
                    outputPin_wirable.LogicState = inputPin_wirable_A.LogicState;
                }
                else if (inputPin_wirable_I.LogicState == LogicState.On)
                {
                    outputPin_wirable.LogicColor = inputPin_wirable_B.LogicColor;
                    outputPin_wirable.LogicState = inputPin_wirable_B.LogicState;
                }
            }
            else if (inputPin_wirable_J.LogicState == LogicState.On)
            {
                if (inputPin_wirable_I.LogicState == LogicState.Off)
                {
                    outputPin_wirable.LogicColor = inputPin_wirable_C.LogicColor;
                    outputPin_wirable.LogicState = inputPin_wirable_C.LogicState;
                }
                else if (inputPin_wirable_I.LogicState == LogicState.On)
                {
                    outputPin_wirable.LogicColor = inputPin_wirable_D.LogicColor;
                    outputPin_wirable.LogicState = inputPin_wirable_D.LogicState;
                }
            }
        }
        else if (inputPin_wirable_K.LogicState == LogicState.On)
        {
            if (inputPin_wirable_J.LogicState == LogicState.Off)
            {
                if (inputPin_wirable_I.LogicState == LogicState.Off)
                {
                    outputPin_wirable.LogicColor = inputPin_wirable_E.LogicColor;
                    outputPin_wirable.LogicState = inputPin_wirable_E.LogicState;
                }
                else if (inputPin_wirable_I.LogicState == LogicState.On)
                {
                    outputPin_wirable.LogicColor = inputPin_wirable_F.LogicColor;
                    outputPin_wirable.LogicState = inputPin_wirable_F.LogicState;
                }
            }
            else if (inputPin_wirable_J.LogicState == LogicState.On)
            {
                if (inputPin_wirable_I.LogicState == LogicState.Off)
                {
                    outputPin_wirable.LogicColor = inputPin_wirable_G.LogicColor;
                    outputPin_wirable.LogicState = inputPin_wirable_G.LogicState;
                }
                else if (inputPin_wirable_I.LogicState == LogicState.On)
                {
                    outputPin_wirable.LogicColor = inputPin_wirable_H.LogicColor;
                    outputPin_wirable.LogicState = inputPin_wirable_H.LogicState;
                }
            }
        }
    }

    // C# Private Methods: /////////////////////////////////////////////////////
  
} // end of class