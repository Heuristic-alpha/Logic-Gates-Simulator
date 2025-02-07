using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HSCL;

public class ContextMenuScreen : HSCL.ScreenBase
{
    GameObject _currentSellectedObject;
    Vector3 _screenPos;
    Camera _camera;
    [SerializeField] GameObject contextMenuObject;
    [SerializeField] TMP_Text headerText;

    public override void OnCreate()
    {
        base.OnCreate();
        _screenSample = HSCL.ScreenSample.ContextMenuScreen;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressExit_Button();
        }
    }

    public void Init(GameObject currentSellectedObject, Vector3 screenPos , Camera cam)
    {
        _currentSellectedObject = currentSellectedObject;
        headerText.text = _currentSellectedObject.name;
        _screenPos = screenPos;
        _camera = cam;
        contextMenuObject.transform.position = CalculateContextMenuPosition(_screenPos);

        // disable keyboard camera movement:
        GameManager.Instance.Enable_MoveCameraWithKeyboard(false);
    }

    public void Press_Delete_Button()
    {
        Destroy(_currentSellectedObject);
        PressExit_Button();
    }
    public void Press_RotatePlus90_Button()
    {
        //_currentSellectedObject.transform.Rotate(Vector3.forward, -90, Space.World);
        //_currentSellectedObject.GetComponent<IBorderable>().GetBorder().RotatePoints_Minus90();
        //HideContextMenu();
        _currentSellectedObject.GetComponent<IItemable>().RotateRight();
        PressExit_Button();
    }
    public void Press_RotateMinus90_Button()
    {
        //_currentSellectedObject.transform.Rotate(Vector3.forward, +90, Space.World);
        //_currentSellectedObject.GetComponent<IBorderable>().GetBorder().RotatePoints_Plus90();
        //HideContextMenu();
        _currentSellectedObject.GetComponent<IItemable>().RotateLeft();
        PressExit_Button();
    }
    public void PressExit_Button()
    {
        UIManager.Singeleton.DestroyTheFrontScreen();

        // enable keyboard camera movement:
        GameManager.Instance.Enable_MoveCameraWithKeyboard(true);
    }

    public void OnPointerEnterOnRotateButton(Image image)
    {
        image.color = Color.red;
    }
    public void OnPointerExitOnRotateButton(Image image)
    {
        image.color = Color.black;
    }

    private Vector3 GetPositionToElementInWorld(Vector3 screenPos)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPos, _camera, out pos);
        return _canvas.transform.TransformPoint(pos);
    }
    private Vector2 GetPositionToElementInCanvas(Vector3 screenPos)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPos, _camera, out pos);
        return pos;
    }
    private Vector3 CalculateContextMenuPosition(Vector3 screenPos)
    {
        RectTransform contextRect = contextMenuObject.transform as RectTransform;
        RectTransform canvasRect = _canvas.transform as RectTransform;
        Vector2 returnPos;

        Vector2 mousePosInCanvas = GetPositionToElementInCanvas(screenPos);
        Vector2 mousePosInCanvasAsScreenSpace = mousePosInCanvas + new Vector2(canvasRect.rect.width / 2, canvasRect.rect.height / 2); // only if parentCanvas pivot be x=0.5 y=0.5

        bool isRectExceededCanvasRight = false;
        bool isRectExceededCanvasBottom = false;

        if (mousePosInCanvasAsScreenSpace.x >= canvasRect.rect.width - contextRect.rect.width) isRectExceededCanvasRight = true;
        if (mousePosInCanvasAsScreenSpace.y <= 0 + contextRect.rect.height) isRectExceededCanvasBottom = true;

        if (isRectExceededCanvasBottom == false && isRectExceededCanvasRight == false)
            returnPos = mousePosInCanvas;

        else if (isRectExceededCanvasBottom == true && isRectExceededCanvasRight == false)
            returnPos = mousePosInCanvas + new Vector2(0, contextRect.rect.height);

        else if (isRectExceededCanvasBottom == false && isRectExceededCanvasRight == true)
            returnPos = mousePosInCanvas + new Vector2(-contextRect.rect.width, 0);

        else
            returnPos = mousePosInCanvas + new Vector2(-contextRect.rect.width, contextRect.rect.height);


        return _canvas.transform.TransformPoint(returnPos);
    }
}
