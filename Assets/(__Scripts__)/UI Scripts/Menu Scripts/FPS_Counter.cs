using UnityEngine;
using TMPro;

public class FPS_Counter : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject FPS_Counter_Panel;

    // Unity Components: ///////////////////////////////////////////////////////
    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] TMP_Text FPS_Text;

    private int currentFPS;
    private float lastTime;
    private const int interval = 1;
    // Unity Main Events: //////////////////////////////////////////////////////
    private void Update()
    {
        currentFPS++;
        float delta = Time.realtimeSinceStartup - lastTime;
        if (delta >= interval)
        {
            FPS_Text.text = $"FPS: {currentFPS}";
            lastTime = Time.realtimeSinceStartup;
            currentFPS = 0;
        }
   
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    // C# Private Methods: /////////////////////////////////////////////////////


} // end of class
