using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject[] inputPins;
    [SerializeField] GameObject[] outputPins;

    // Unity Components: ///////////////////////////////////////////////////////
    IWirable[] _inputPins_IwirableComponents;
    IWirable[] _outputPins_IwirableComponents;

    // C# Properties: //////////////////////////////////////////////////////////
    public int InputPinsLength {  get { return inputPins.Length; } }
    public int OutputPinsLength { get { return outputPins.Length; } }

    // C# Fields: //////////////////////////////////////////////////////////////
    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _inputPins_IwirableComponents = new IWirable[inputPins.Length];
        _outputPins_IwirableComponents = new IWirable[outputPins.Length];
        Init_IWirable_Array();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////

    public GameObject GetInputPin(int index)
    {
        if(index < 0 || index >= inputPins.Length)
        {
            Debug.LogError("index out of range");
            return null;
        }
        return inputPins[index];
    }
    public GameObject GetOutputPin(int index)
    {
        if (index < 0 || index >= outputPins.Length)
        {
            Debug.LogError("index out of range");
            return null;
        }      
        return outputPins[index];
    }
    public GameObject[] GetInputPins() => inputPins;
    public GameObject[] GetOutputPins() => outputPins;
    public IWirable GetInputPin_IWirable(int index)
    {
        //if (index < 0 || index >= _inputPins_IwirableComponents.Length)
        //{
        //    Debug.LogError("index out of range");
        //    return null;
        //}
        return _inputPins_IwirableComponents[index];
    }
    public IWirable GetOutputPin_IWirable(int index)
    {
        //if (index < 0 || index >= _outputPins_IwirableComponents.Length)
        //{
        //    Debug.LogError("index out of range");
        //    return null;
        //}
        return _outputPins_IwirableComponents[index];
    }
    public IWirable[] GetInputPin_IWirables() => _inputPins_IwirableComponents;
    public IWirable[] GetOutputPin_IWirables() => _outputPins_IwirableComponents;
    public void UpdateAllPinsVisual()
    {
        // update input pins:
        foreach (var pin in _inputPins_IwirableComponents) pin.UpdateVisual();

        // update output pins:
        foreach(var pin in _outputPins_IwirableComponents) pin .UpdateVisual();
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void Init_IWirable_Array()
    {
        for (int i = 0; i < inputPins.Length; i++)
        {
            _inputPins_IwirableComponents[i] = inputPins[i].GetComponent<IWirable>();
        }
        for (int i = 0; i < outputPins.Length; i++)
        {
            _outputPins_IwirableComponents[i] = outputPins[i].GetComponent<IWirable>();
        }
    }

    
} // end of class
