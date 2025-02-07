using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogicHelper
{
    public static LogicState BUFFER(LogicState input)
    {
        return input;
    }
    public static LogicState NOT1(LogicState input)
    {
        if (input == LogicState.On)
        {
            return LogicState.Off;
        }
        else if (input == LogicState.Off)
        {
            return LogicState.On;
        }
        else
        {
            Debug.LogError("Error: invalid input");
            return LogicState.None;
        }
    }
    public static LogicState AND2(LogicState stateA, LogicState stateB)
    {
        if (stateA == LogicState.On && stateB == LogicState.On)
        {
            return LogicState.On;
        }
        else
        {
            return LogicState.Off;
        }
    }
    public static LogicState OR2(LogicState stateA, LogicState stateB)
    {
        if (stateA == LogicState.On || stateB == LogicState.On)
        {
            return LogicState.On;
        }
        else
        {
            return LogicState.Off;
        }
    }
    public static LogicState NAND2(LogicState stateA, LogicState stateB)
    {
        if (stateA == LogicState.On && stateB == LogicState.On)
        {
            return NOT1(LogicState.On);
        }
        else
        {
            return NOT1(LogicState.Off);
        }
    }
    public static LogicState NOR2(LogicState stateA, LogicState stateB)
    {
        if (stateA == LogicState.On || stateB == LogicState.On)
        {
            return NOT1(LogicState.On);
        }
        else
        {
            return NOT1(LogicState.Off);
        }
    }
    public static LogicState XOR2(LogicState stateA, LogicState stateB)
    {
        if (stateA == LogicState.On)
        {
            if (stateB == LogicState.Off) return LogicState.On;
            else if (stateB == LogicState.On) return LogicState.Off;
            else
            {
                Debug.LogError("Error: invalid input");
                return LogicState.None;
            }

        }
        else if (stateA == LogicState.Off)
        {
            if (stateB == LogicState.Off) return LogicState.Off;
            else if (stateB == LogicState.On) return LogicState.On;
            else
            {
                Debug.LogError("Error: invalid input");
                return LogicState.None;
            }
        }
        else
        {
            Debug.LogError("Error: invalid input");
            return LogicState.None;
        }
    }
    public static LogicState XNOR2(LogicState stateA, LogicState stateB)
    {
        if (stateA == LogicState.On)
        {
            if (stateB == LogicState.Off) return NOT1(LogicState.On);
            else if (stateB == LogicState.On) return NOT1(LogicState.Off);
            else
            {
                Debug.LogError("Error: invalid input");
                return LogicState.None;
            }

        }
        else if (stateA == LogicState.Off)
        {
            if (stateB == LogicState.Off) return NOT1(LogicState.Off);
            else if (stateB == LogicState.On) return NOT1(LogicState.On);
            else
            {
                Debug.LogError("Error: invalid input");
                return LogicState.None;
            }
        }
        else
        {
            Debug.LogError("Error: invalid input");
            return LogicState.None;
        }
    }

}// end of class