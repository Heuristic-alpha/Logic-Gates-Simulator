using UnityEngine;

public class WindowScreenBase : HSCL.ScreenBase
{

    [SerializeField] protected GameObject _blockInteractionPanel;
    [SerializeField] protected GameObject _windowPanel;
    [SerializeField] protected bool _dragableWindow;
    public bool IsInteractive { get; private set; }

    public override void OnCreate()
    {
        base.OnCreate();
        _screenSample = HSCL.ScreenSample.WindowScreenBase;       
    }

    public override void OnOpen()
    {
        base.OnOpen();
        windowPanelLastWorldPos = _windowPanel.transform.position;
    }

    public override void OnFocus()
    {
        base.OnFocus();
        SetWindowInteraction(true);
        GameManager.Instance.Enable_MoveCameraWithKeyboard(false);
    }
    public override void OnFocusLost()
    {
        base.OnFocusLost();
        SetWindowInteraction(false);
        GameManager.Instance.Enable_MoveCameraWithKeyboard(true);
    }

    public override void OnClosedByUIManager(bool cashed)
    {
        base.OnClosedByUIManager(cashed);
        if (cashed)
        {
            // reset all value:
            windowPanelLastWorldPos = Vector3.zero;
            _windowPanel.transform.localPosition = Vector3.zero;
            clickStartPos = Vector2.zero;
            clickEndPos = Vector2.zero;
            pointerIsMoved = false;
        }
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Enable_MoveCameraWithKeyboard(true);
            UIManager.Singeleton.CloseTheFrontScreen();
        }

        if(pointerIsMoved && _dragableWindow)
        {
            OnDrag();
        }

    }

    public void SetWindowInteraction(bool interactivity)
    {
        IsInteractive = interactivity;
        _blockInteractionPanel.SetActive(!interactivity);
    }

    public virtual void OnClickCloseWindowButton()
    {
        GameManager.Instance.Enable_MoveCameraWithKeyboard(true);
        UIManager.Singeleton.CloseTheFrontScreen();
    }


    Vector2 clickStartPos;
    Vector2 clickEndPos;
    Vector3 windowPanelLastWorldPos;
    bool pointerIsMoved;

    public void OnPointerDown()
    {
        if(!_dragableWindow) return;

        pointerIsMoved = true;
        clickStartPos = GetPositionToElementInWorld(Input.mousePosition);
        _windowPanel.transform.position = windowPanelLastWorldPos;
        GameManager.Instance.CursorManager.SetToGrabbedHandCursor();
    }
    public void OnPointerUp() 
    {
        if (!_dragableWindow) return;

        pointerIsMoved = false;
        windowPanelLastWorldPos = _windowPanel.transform.position;
        GameManager.Instance.CursorManager.SetToDefaultCursor();
    }  
    private void OnDrag()
    {
        clickEndPos = GetPositionToElementInWorld(Input.mousePosition);
        Vector3 delta = clickEndPos - clickStartPos;
        _windowPanel.transform.position = windowPanelLastWorldPos + delta;

        // fix some problem with random z value:
        Vector3 localPos = _windowPanel.transform.localPosition;
        localPos.z = 0;
        _windowPanel.transform.localPosition = localPos;

    }   
    private Vector2 GetPositionToElementInCanvas(Vector3 screenPos)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPos, _canvas.worldCamera, out pos);
        return pos;
    }
    private Vector3 GetPositionToElementInWorld(Vector3 screenPos)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPos, _canvas.worldCamera, out pos);
        return _canvas.transform.TransformPoint(pos);
    }
   
}
