using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenSegmentDriver : ChipBase
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    IWirable inputPin_wirable_A;
    IWirable inputPin_wirable_B;
    IWirable inputPin_wirable_C;
    IWirable inputPin_wirable_D;

    IWirable outputPin_wirable_A;
    IWirable outputPin_wirable_B;
    IWirable outputPin_wirable_C;
    IWirable outputPin_wirable_D;
    IWirable outputPin_wirable_E;
    IWirable outputPin_wirable_F;
    IWirable outputPin_wirable_G;

    // Unity Main Events: //////////////////////////////////////////////////////
    protected override void Awake()
    {
        base.Awake();

        inputPin_wirable_A = _pinManager.GetInputPin_IWirable(0);
        inputPin_wirable_B = _pinManager.GetInputPin_IWirable(1);
        inputPin_wirable_C = _pinManager.GetInputPin_IWirable(2);
        inputPin_wirable_D = _pinManager.GetInputPin_IWirable(3);

        outputPin_wirable_A = _pinManager.GetOutputPin_IWirable(0);
        outputPin_wirable_B = _pinManager.GetOutputPin_IWirable(1);
        outputPin_wirable_C = _pinManager.GetOutputPin_IWirable(2);
        outputPin_wirable_D = _pinManager.GetOutputPin_IWirable(3);
        outputPin_wirable_E = _pinManager.GetOutputPin_IWirable(4);
        outputPin_wirable_F = _pinManager.GetOutputPin_IWirable(5);
        outputPin_wirable_G = _pinManager.GetOutputPin_IWirable(6);
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public override void HandleLogic()
    {
        if(inputPin_wirable_D.LogicState == LogicState.On)
        {
            if (inputPin_wirable_C.LogicState == LogicState.On)
            {
                if (inputPin_wirable_B.LogicState == LogicState.On)
                {
                    if (inputPin_wirable_A.LogicState == LogicState.On) // F
                    {
                        SetOutput(true, false, false, false, true, true, true);
                    }
                    else // E
                    {
                        SetOutput(true, false, false, true, true, true, true);
                    }
                }
                else
                {
                    if (inputPin_wirable_A.LogicState == LogicState.On) // d
                    {
                        SetOutput(false, true, true, true, true, false, true);
                    }
                    else // C
                    {
                        SetOutput(true, false, false, true, true, true, false);
                    }
                }
            }
            else
            {
                if (inputPin_wirable_B.LogicState == LogicState.On)
                {
                    if (inputPin_wirable_A.LogicState == LogicState.On) // b
                    {
                        SetOutput(false, false, true, true, true, true, true);
                    }
                    else // A
                    {
                        SetOutput(true, true, true, false, true, true, true);
                    }
                }
                else
                {
                    if (inputPin_wirable_A.LogicState == LogicState.On) // 9
                    {
                        SetOutput(true, true, true, true, false, true, true);
                    }
                    else // 8
                    {
                        SetOutput(true, true, true, true, true, true, true);
                    }
                }
            }
        }
        else
        {
            if (inputPin_wirable_C.LogicState == LogicState.On)
            {
                if (inputPin_wirable_B.LogicState == LogicState.On)
                {
                    if (inputPin_wirable_A.LogicState == LogicState.On) // 7
                    {
                        SetOutput(true, true, true, false, false, false, false);
                    }
                    else // 6
                    {
                        SetOutput(true, false, true, true, true, true, true);
                    }
                }
                else
                {
                    if (inputPin_wirable_A.LogicState == LogicState.On) // 5
                    {
                        SetOutput(true, false, true, true, false, true, true);
                    }
                    else // 4
                    {
                        SetOutput(false, true, true, false, false, true, true);
                    }
                }
            }
            else
            {
                if (inputPin_wirable_B.LogicState == LogicState.On)
                {
                    if (inputPin_wirable_A.LogicState == LogicState.On) // 3
                    {
                        SetOutput(true, true, true, true, false, false, true);
                    }
                    else // 2
                    {
                        SetOutput(true, true, false, true, true, false, true);
                    }
                }
                else
                {
                    if (inputPin_wirable_A.LogicState == LogicState.On) // 1
                    {
                        SetOutput(false, true, true, false, false, false, false);
                    }
                    else // 0
                    {
                        SetOutput(true, true, true, true, true, true, false);
                    }
                }
            }
        }
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void SetOutput(bool A, bool B, bool C, bool D, bool E, bool F, bool G)
    {
        outputPin_wirable_A.LogicState = A ? LogicState.On : LogicState.Off;
        outputPin_wirable_B.LogicState = B ? LogicState.On : LogicState.Off;
        outputPin_wirable_C.LogicState = C ? LogicState.On : LogicState.Off;
        outputPin_wirable_D.LogicState = D ? LogicState.On : LogicState.Off;
        outputPin_wirable_E.LogicState = E ? LogicState.On : LogicState.Off;
        outputPin_wirable_F.LogicState = F ? LogicState.On : LogicState.Off;
        outputPin_wirable_G.LogicState = G ? LogicState.On : LogicState.Off;
    }


} // end of class