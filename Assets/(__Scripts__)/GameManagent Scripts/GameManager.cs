using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] MouseInputAction _mouseInputActionScript;
    [SerializeField] ObjectToGridPosition _objectToGridPosition;
    [SerializeField] MoveCameraWithKeyboard _moveCameraWithKeyboardScript;
    [SerializeField] BackGroundGridController _backGroundGridController;
    [SerializeField] CursorManager _cursorManager;

    // C# Properties: //////////////////////////////////////////////////////////
    public static GameManager Instance { get { return _instance; } }
    public Border BackGroundBorder { get { return _backGroundBorder; } }
    public CursorManager CursorManager { get { return _cursorManager; } }

    public Color LogicState_On_Color { get { return _logicState_On_Color; } }
    public Color LogicState_Off_Color { get { return _logicState_Off_Color; } }
    public LogicColor LogicDefaultColor { get { return _logicDefaultColor; } }

    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] Color _logicState_On_Color;
    [SerializeField] Color _logicState_Off_Color;   
    private LogicColor _logicDefaultColor;
    
    private static GameManager _instance;
    [SerializeField] Border _backGroundBorder;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        //singeleton:
        if (_instance != null) { Destroy(this); return; }
        else _instance = this;

        _logicDefaultColor = new LogicColor(_logicState_On_Color, _logicState_Off_Color);

       Init_FPS();
    }

    private void Start()
    {
        UIManager.Singeleton.CreateScreen(HSCL.ScreenSample.GamePlayScreen);
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////     

    public void Enable_MouseInputAction(bool answer)
    {
        _mouseInputActionScript.enabled = answer;
    }
    public void Enable_MoveCameraWithKeyboard(bool answer)
    {
        _moveCameraWithKeyboardScript.enabled = answer;
    }
   
    public  void Init_FPS()
    {

#if UNITY_ANDROID || UNITY_IOS
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
#else 
        Application.targetFrameRate = -1;
#endif

    }

    public ObjectToGridPosition GetObjectToGridPositionScript() => _objectToGridPosition;
    public BackGroundGridController GetBackGroundGridControllerScript() => _backGroundGridController;

    // C# Private Methods: /////////////////////////////////////////////////////
    
}// class end
