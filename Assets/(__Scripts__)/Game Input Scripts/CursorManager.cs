using UnityEngine;

public class CursorManager : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    // C# Properties: //////////////////////////////////////////////////////////
    public bool IsCompatible { get; private set; }

    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] Texture2D _cursor_Default;
    [SerializeField] Texture2D _cursor_Hand;
    [SerializeField] Texture2D _cursor_GrabbedHand;
    [SerializeField] Texture2D _cursor_CameraMove;

    int curentRunningPlatform = -1;
    int androidSDKVersion = -1;

    // Unity Main Events: //////////////////////////////////////////////////////

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        SetToDefaultCursor();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void SetToDefaultCursor()
    {
        if (!IsCompatible) return;
        Cursor.SetCursor(_cursor_Default, new Vector2(2, 2), CursorMode.Auto);
    }
    public void SetToHandCursor()
    {
        if (!IsCompatible) return;
        Cursor.SetCursor(_cursor_Hand, new Vector2(25, 0), CursorMode.Auto);
    }
    public void SetToGrabbedHandCursor()
    {
        if (!IsCompatible) return;
        Cursor.SetCursor(_cursor_GrabbedHand, new Vector2(20, 20), CursorMode.Auto);
    }
    public void SetToMoveCameraCursor()
    {
        if (!IsCompatible) return;
        Cursor.SetCursor(_cursor_CameraMove, new Vector2(31, 31), CursorMode.Auto);
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void Init()
    {
#if UNITY_EDITOR
        curentRunningPlatform = 0;
#elif UNITY_STANDALONE
        curentRunningPlatform = 1;
#elif UNITY_WEBGL
        curentRunningPlatform = 2;
#elif UNITY_ANDROID
        curentRunningPlatform = 3;
        AndroidJavaClass androidVerInfo = new AndroidJavaClass("android.os.Build$VERSION");
        androidSDKVersion = androidVerInfo.GetStatic<int>("SDK_INT");       
#endif

        IsCompatible = CheckCompatibility();
    }

    private bool CheckCompatibility()
    {
        if (curentRunningPlatform == 0) return true;
        else if (curentRunningPlatform == 1) return true;
        else if (curentRunningPlatform == 2) return false;
        else if (curentRunningPlatform == 3) // android
        {
            if (androidSDKVersion >= 29)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.LogError("Cursor Manager UnCompatibile platform");
            return false;
        }
    }


} // end of class