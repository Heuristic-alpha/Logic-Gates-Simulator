using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour, IBorderable
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] Border _border;
    [SerializeField] Camera cam;

    // C# Properties: //////////////////////////////////////////////////////////
    public float CameraCurrentSize { get { return cam.orthographicSize; } }
    public float CameraMaxSize { get { return _camMaxSize; } }
    public float CameraMinSize { get { return _camMinSize; } }
    public bool IsMoving { get => _isMoving; set { _isMoving = value; } }
    
    // C# Fields: //////////////////////////////////////////////////////////////
    private int _screenLastWidth;
    private int _screenLastHeight;
    private Border _camBorder;
    private bool _isMoving = false;
    [SerializeField] float _camMaxSize;
    [SerializeField] float _camMinSize;
    [SerializeField] float _camMouseZoomSpeed;

    private float _touchDistanceTolerance = 2;
    private float _camTouchZoomSpeed = 0.02f;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _camBorder = GetComponentInChildren<Border>();
    }
    private void Start()
    {
        SetCameraBorder();
    }

    private void Update()
    {
        UpdateCameraBorder();
        ZoomCameraWithMouse();
        ZoomCameraWith2Touch();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////   
    public Border GetBorder()
    {
        return _camBorder;
    }

    public float GetCameraSize() => cam.orthographicSize;

    public void SetCameraSizeExactly(float size)
    {
        cam.orthographicSize = size;

        SetCameraBorder();
        Border.CheckAndResolve_BorderInBackGroundBorder(_camBorder);
    }

    public void SetCameraPos(Vector2 pos)
    {
        cam.transform.position = new Vector3(pos.x, pos.y, cam.transform.position.z);
    }
    
    // C# Private Methods: /////////////////////////////////////////////////////

    private void SetCameraBorder()
    {
        float yOffset = cam.orthographicSize;
        float xOffset = yOffset * cam.aspect;

        _border.SetBorderUp(new Vector2(0,yOffset));
        _border.SetBorderDown(new Vector2(0,-yOffset));
        _border.SetBorderLeft(new Vector2(-xOffset,0));
        _border.SetBorderRight(new Vector2(xOffset,0));
    }
    private void UpdateCameraBorder()
    {
        if ((Screen.width != _screenLastWidth) || (Screen.height != _screenLastHeight))
        {
            SetCameraBorder();
            Border.CheckAndResolve_BorderInBackGroundBorder(_camBorder);
            _screenLastHeight = Screen.height;
            _screenLastWidth = Screen.width;
        }       
    }
    private void ZoomCameraWithMouse()
    {
        if (EventSystem.current.IsPointerOverGameObject(MouseInputAction.EVENT_SYSTEM_MOUSE_ID)) return;

        if (Input.mouseScrollDelta.y != 0)
        {
            float tempSize = -Input.mouseScrollDelta.y * Time.deltaTime * _camMouseZoomSpeed;
            tempSize += cam.orthographicSize;
            cam.orthographicSize = Mathf.Clamp(tempSize, _camMinSize, _camMaxSize);

            SetCameraBorder();
            Border.CheckAndResolve_BorderInBackGroundBorder(_camBorder);
        }
    }
    private void ZoomCameraWith2Touch()
    {     
        if (Input.touchCount == 2)
        {         
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(1)) return;

            // Find the position in the previous frame of each touch.
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentTouchDeltaMag = (touch0.position - touch1.position).magnitude;

            // Find the difference in the distances between each frame.
            float delta = prevTouchDeltaMag - currentTouchDeltaMag;

            if (Mathf.Abs(delta) < _touchDistanceTolerance) return;

            float tempSize = delta * _camTouchZoomSpeed;
            tempSize += cam.orthographicSize;
            cam.orthographicSize = Mathf.Clamp(tempSize, _camMinSize, _camMaxSize);

            SetCameraBorder();
            Border.CheckAndResolve_BorderInBackGroundBorder(_camBorder);
        }
    }
}