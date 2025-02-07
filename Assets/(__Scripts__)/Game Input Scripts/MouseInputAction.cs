using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class MouseInputAction : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] CursorManager _cursorManager;
    ObjectToGridPosition _objectToGridPosition;

    // C# Constant: ////////////////////////////////////////////////////////////
    public const int EVENT_SYSTEM_MOUSE_ID = -1;
    public const int EVENT_SYSTEM_TOUCH_1_ID = 0;

    // C# Properties: //////////////////////////////////////////////////////////
    public MouseAction CurrentMouseAction { get => _currentMouseAction; }

    // C# Fields: //////////////////////////////////////////////////////////////
    [Header("Setting:")]
    [SerializeField] KeyCode primaryKey = KeyCode.Mouse0;
    [SerializeField] KeyCode cancelKey = KeyCode.Mouse1;

    private MouseAction _currentMouseAction;
    private Vector3 currentMouseWorldPosition;
    private Camera cam;
    private int layerMask;

    #region _currentMouseAction.MoveObject Fields
    private GameObject objectToMove;
    private Border objectToMoveBorder;
    private Vector3 objectToMoveOffsetPosition;

    private SortingGroup objectToMoveSortingGroup;
    private int objectToMoveSortingOrderMoveOffset = 2;
    
    #endregion

    #region _currentMouseAction.MoveCam Fields
    private Vector3 clickedMouseWorldPosition;
    private Border camBorder;
    private float camMoveSpeedOnScreenEdge = 1f;
    #endregion

    #region _currentMouseAction.PlaceWire & MoveWire Fields
    [Header("Wires:")]
    [SerializeField] GameObject _wirePrefab;
    [SerializeField] GameObject _previewWirePrefab;
    private PreviewWire _previewWireComponent;
    private GameObject _previewWireObject;
    private GameObject _wireStartPoint;
    private GameObject _wireEndPoint;
    private Trait _currentTraitOfSellectedWirePoint = Trait.None;
    private GameObject _sellectedWireToMoveGameObject;

    #endregion


    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _currentMouseAction = MouseAction.None;
        _objectToGridPosition = GetComponentInParent<ObjectToGridPosition>();
        cam = Camera.main;
        camBorder = cam.GetComponent<IBorderable>().GetBorder();
        layerMask = 1 << LayerMask.NameToLayer("Logic");
    }
    private void Start()
    {
        SpawnPreviewWire();
    }
    private void Update()
    {
        /*
         This script only support one pointer or one touch,
         so if more than one touch register on screen, we return back           
         */
        if (Input.touchCount > 1)
        {
            CancelThisMouseAction(_currentMouseAction);
            return;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        currentMouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition); currentMouseWorldPosition.z = 0;
        //////////////// Mouse GetKeyDown ///////////////////
        if (Input.GetKeyDown(primaryKey))
        {
            if (EventSystem.current.IsPointerOverGameObject(EVENT_SYSTEM_MOUSE_ID)
             || EventSystem.current.IsPointerOverGameObject(EVENT_SYSTEM_TOUCH_1_ID)) return;
            OperationOnMouseKetKeyDown();
        }
        //////////////// Mouse GetKey ///////////////////
        if (Input.GetKey(primaryKey))
        {
            OperationOnMouseKetKey();
        }
        //////////////// Mouse GetKeyUp ///////////////////
        if (Input.GetKeyUp(primaryKey))
        {
            OperationOnMouseKetKeyUp();
        }
    }
    // Unity Other Events: /////////////////////////////////////////////////////
    private void OnDisable()
    {
        CancelThisMouseAction(_currentMouseAction);
    }

    // C# Public Methods: //////////////////////////////////////////////////////   

    // C# Private Methods: /////////////////////////////////////////////////////
    private void OperationOnMouseKetKeyDown()
    {
        GameObject go = RayCastAndGetToppestGameobject(currentMouseWorldPosition);
        if (go == null)
        {
            clickedMouseWorldPosition = currentMouseWorldPosition;
            SetMouseAction(MouseAction.MoveCam);
            return;
        }
        else
        {
            TraitHolder traitHolder = go.GetComponent<TraitHolder>();
            if (traitHolder == null) { Debug.LogError($"Gameobject( {go.name} ) dont have any TraitHolder Component!"); return; }
            else
            {
                if (traitHolder.Contain(Trait.CanMove) && traitHolder.Contain(Trait.HaveBorder)) // if we click on movableObject that have border **************
                {
                    IBorderable borderable = go.GetComponent<IBorderable>();
                    if (borderable.IsMoving) return;

                    ShowObjectInFront(go);

                    objectToMove = go;
                    objectToMoveBorder = borderable.GetBorder();
                    objectToMoveOffsetPosition = objectToMove.transform.position - currentMouseWorldPosition;
                    SetMouseAction(MouseAction.MoveObject);
                    return;
                }
                else if (traitHolder.Contain(Trait.VirutalButton)) // if we click on VirutalButton *************
                {
                    go.GetComponentInParent<IClickableVirutalButton>().ClickVirutalButton();
                    SetMouseAction(MouseAction.ClickOnVirutalButton);
                }
                else if (traitHolder.Contain(Trait.Wirable)) // if we click on Pin *****************************
                {
                    IWirable wireHelperPoint = go.GetComponent<IWirable>();

                    if (wireHelperPoint.Wire_GameObject == null) // if pin don't attached to any wire, we should Place Wire:
                    {
                        if (traitHolder.Contain(Trait.Output))
                        {
                            // Wire:
                            _wireStartPoint = go;

                            // PreviewWire:
                            _previewWireObject.SetActive(true);
                            _previewWireComponent.SetStartPoint_Position(go.transform.position);
                            _previewWireComponent.SetStartPoint_Rotation(go.transform.rotation);
                            _previewWireComponent.SetEndPoint_Position(currentMouseWorldPosition);
                            _previewWireComponent.SetEndPoint_Rotation(Quaternion.identity);
                            _previewWireComponent.IsPreviewWireStartFromStartPoint = true;
                            _currentTraitOfSellectedWirePoint = Trait.Output;
                            _previewWireObject.GetComponent<IColorable>().LogicColor = _wireStartPoint.GetComponent<IColorable>().LogicColor;
                            _previewWireComponent.UpdateWire();

                            SetMouseAction(MouseAction.PlaceWire);
                        }
                        else if (traitHolder.Contain(Trait.Input))
                        {
                            // Wire:
                            _wireEndPoint = go;

                            // PreviewWire:
                            _previewWireObject.SetActive(true);
                            _previewWireComponent.SetEndPoint_Position(go.transform.position);
                            _previewWireComponent.SetEndPoint_Rotation(go.transform.rotation);
                            _previewWireComponent.SetStartPoint_Position(currentMouseWorldPosition);
                            _previewWireComponent.SetStartPoint_Rotation(Quaternion.identity);
                            _previewWireComponent.IsPreviewWireStartFromStartPoint = false;
                            _currentTraitOfSellectedWirePoint = Trait.Input;
                            _previewWireObject.GetComponent<IColorable>().LogicColor = _wireEndPoint.GetComponent<IColorable>().LogicColor;
                            _previewWireComponent.UpdateWire();

                            SetMouseAction(MouseAction.PlaceWire);
                        }
                    }
                    else // if pin already attached to a wire, we should Move Wire:
                    {
                        if (traitHolder.Contain(Trait.Output))
                        {
                            // Wire:
                            _wireStartPoint = go;
                            _currentTraitOfSellectedWirePoint = Trait.Output;
                            _sellectedWireToMoveGameObject = wireHelperPoint.Wire_GameObject;
                            _sellectedWireToMoveGameObject.SetActive(false);
                            Wire currentWireComponent = wireHelperPoint.Wire_GameObject.GetComponent<Wire>();
                            _wireEndPoint = currentWireComponent.EndPoint;

                            // PreviewWire:
                            _previewWireObject.SetActive(true);
                            _previewWireComponent.SetEndPoint_Position(currentWireComponent.EndPoint.transform.position);
                            _previewWireComponent.SetEndPoint_Rotation(currentWireComponent.EndPoint.transform.rotation);
                            _previewWireComponent.SetStartPoint_Position(currentMouseWorldPosition);
                            _previewWireComponent.SetStartPoint_Rotation(Quaternion.identity);
                            _previewWireComponent.IsPreviewWireStartFromStartPoint = false;
                            _previewWireComponent.LogicColor = _sellectedWireToMoveGameObject.GetComponent<IColorable>().LogicColor;
                            _previewWireComponent.UpdateWire();

                            SetMouseAction(MouseAction.MoveWire);
                        }
                        else if (traitHolder.Contain(Trait.Input))
                        {
                            // Wire:
                            _wireEndPoint = go;
                            _currentTraitOfSellectedWirePoint = Trait.Input;
                            _sellectedWireToMoveGameObject = wireHelperPoint.Wire_GameObject;
                            _sellectedWireToMoveGameObject.SetActive(false);
                            Wire currentWireComponent = wireHelperPoint.Wire_GameObject.GetComponent<Wire>();
                            _wireStartPoint = currentWireComponent.StartPoint;

                            // PreviewWire:
                            _previewWireObject.SetActive(true);
                            _previewWireComponent.SetStartPoint_Position(currentWireComponent.StartPoint.transform.position);
                            _previewWireComponent.SetStartPoint_Rotation(currentWireComponent.StartPoint.transform.rotation);
                            _previewWireComponent.SetEndPoint_Position(currentMouseWorldPosition);
                            _previewWireComponent.SetEndPoint_Rotation(Quaternion.identity);
                            _previewWireComponent.IsPreviewWireStartFromStartPoint = true;
                            _previewWireComponent.LogicColor = _sellectedWireToMoveGameObject.GetComponent<IColorable>().LogicColor;
                            _previewWireComponent.UpdateWire();

                            SetMouseAction(MouseAction.MoveWire);
                        }
                    }
                }
            }
        }
    }
    private void OperationOnMouseKetKey()
    {
        switch (_currentMouseAction)
        {
            case MouseAction.None:
                {

                }
                break;

            case MouseAction.MoveObject:
                {
                    // Cancel Current Operation If MouseRightKey(cancelKey) is Pressed:
                    if (Input.GetKey(cancelKey)) { CancelThisMouseAction(_currentMouseAction); return; }

                    objectToMove.transform.position = currentMouseWorldPosition + objectToMoveOffsetPosition;
                    Border.CheckAndResolve_BorderInBackGroundBorder(objectToMoveBorder);
                    MoveCameraOnScreenEdge();
                }
                break;

            case MouseAction.PlaceWire:
                {
                    // Cancel Current Operation If MouseRightKey(cancelKey) is Pressed:
                    if (Input.GetKey(cancelKey)) { CancelThisMouseAction(_currentMouseAction); return; }

                    MoveCameraOnScreenEdge();

                    // update PreviewWire position:
                    if (_currentTraitOfSellectedWirePoint == Trait.Input) _previewWireComponent.SetStartPoint_Position(currentMouseWorldPosition);
                    else if (_currentTraitOfSellectedWirePoint == Trait.Output) _previewWireComponent.SetEndPoint_Position(currentMouseWorldPosition);
                }
                break;

            case MouseAction.MoveWire:
                {
                    // Cancel Current Operation If MouseRightKey(cancelKey) is Pressed:
                    if (Input.GetKey(cancelKey)) { CancelThisMouseAction(_currentMouseAction); return; }

                    MoveCameraOnScreenEdge();

                    // update PreviewWire position:
                    if (_currentTraitOfSellectedWirePoint == Trait.Input) _previewWireComponent.SetEndPoint_Position(currentMouseWorldPosition);
                    else if (_currentTraitOfSellectedWirePoint == Trait.Output) _previewWireComponent.SetStartPoint_Position(currentMouseWorldPosition);
                }
                break;

            case MouseAction.MoveCam:
                {
                    Vector3 distanceToMoveThisFrame = clickedMouseWorldPosition - currentMouseWorldPosition;
                    cam.transform.position += distanceToMoveThisFrame;
                    Border.CheckAndResolve_BorderInBackGroundBorder(camBorder);
                }
                break;

            case MouseAction.ClickOnVirutalButton:
                {

                }
                break;
        }
    }
    private void OperationOnMouseKetKeyUp()
    {
        GameObject go = RayCastAndGetToppestGameobject(currentMouseWorldPosition);
        if (go == null)
        {
            CancelThisMouseAction(_currentMouseAction);
            return;
        }
        else
        {
            TraitHolder traitHolder = go.GetComponent<TraitHolder>();
            if (traitHolder == null)
            {
                Debug.LogError($"Gameobject( {go.name} ) dont have any TraitHolder Component!");
                CancelThisMouseAction(_currentMouseAction);
                return;
            }
            else
            {
                switch (_currentMouseAction)
                {
                    case MouseAction.None:
                        break;
                    case MouseAction.MoveObject:                       
                        CancelThisMouseAction(_currentMouseAction);
                        break;
                    case MouseAction.PlaceWire:
                        {
                            if (traitHolder.Contain(Trait.Wirable))
                            {
                                IWirable wireHelperPoint = go.GetComponent<IWirable>();

                                if (wireHelperPoint.Wire_GameObject == null) // if pin don't attached to any wire
                                {
                                    if (traitHolder.Contain(Trait.Output))
                                    {
                                        _wireStartPoint = go;
                                        // check for equalities of two gameObjects start and end:
                                        if (_wireEndPoint == null || GameObject.ReferenceEquals(_wireStartPoint, _wireEndPoint)) { CancelThisMouseAction(_currentMouseAction); return; }
                                        SpawnWire(_wireStartPoint, _wireEndPoint);
                                    }
                                    else if (traitHolder.Contain(Trait.Input))
                                    {
                                        _wireEndPoint = go;
                                        // check for equalities of two gameObjects start and end:
                                        if (_wireStartPoint == null || GameObject.ReferenceEquals(_wireStartPoint, _wireEndPoint)) { CancelThisMouseAction(_currentMouseAction); return; }
                                        SpawnWire(_wireStartPoint, _wireEndPoint);
                                    }
                                    else // if traitHolder contain Wirable but not any input or output:
                                    {
                                        CancelThisMouseAction(_currentMouseAction); return;
                                    }
                                }
                                else // if pin already attached to a wire, cancel mouseOpertation:
                                {
                                    CancelThisMouseAction(_currentMouseAction); return;
                                }
                            }
                            else
                            {
                                CancelThisMouseAction(_currentMouseAction); return;
                            }
                        }
                        break;

                    case MouseAction.MoveWire:
                        {
                            if (traitHolder.Contain(Trait.Wirable))
                            {
                                IWirable wireHelperPoint = go.GetComponent<IWirable>();

                                if (wireHelperPoint.Wire_GameObject == null)  // if pin don't attached to any wire, then Move the wire
                                {
                                    if (traitHolder.Contain(Trait.Input) && _currentTraitOfSellectedWirePoint == Trait.Output) // if _startPoint and _endPoint are equal, cancel wireMovement
                                    {
                                        CancelThisMouseAction(_currentMouseAction);
                                        //Debug.Log("11111111");
                                        return;
                                    }
                                    else if (traitHolder.Contain(Trait.Output) && _currentTraitOfSellectedWirePoint == Trait.Output)
                                    {
                                        _wireStartPoint.GetComponent<IWirable>().Wire_GameObject = null; // null the last _wireStartPoint before switching to new _wireStartPoint
                                        _wireStartPoint = go;
                                        MoveWire();
                                        //Debug.Log("2222222");
                                    }
                                    else if (traitHolder.Contain(Trait.Output) && _currentTraitOfSellectedWirePoint == Trait.Input) // if _startPoint and _endPoint are equal, cancel wireMovement
                                    {
                                        CancelThisMouseAction(_currentMouseAction);
                                        // Debug.Log("3333333");
                                        return;
                                    }
                                    else if (traitHolder.Contain(Trait.Input) && _currentTraitOfSellectedWirePoint == Trait.Input)
                                    {
                                        _wireEndPoint.GetComponent<IWirable>().Wire_GameObject = null; // null the last _wireEndPoint before switching to new _wireEndPoint
                                        _wireEndPoint = go;
                                        MoveWire();
                                        //Debug.Log("4444444");
                                    }
                                }
                                else // if pin already attached to a wire, then Sawp the Wires:
                                {
                                    GameObject wireGameObject1 = _sellectedWireToMoveGameObject;
                                    GameObject wireGameObject2 = wireHelperPoint.Wire_GameObject;

                                    if (traitHolder.Contain(Trait.Output) && _currentTraitOfSellectedWirePoint == Trait.Output)
                                    {
                                        SwapWire(wireGameObject1, wireGameObject2, true);
                                    }
                                    else if (traitHolder.Contain(Trait.Input) && _currentTraitOfSellectedWirePoint == Trait.Input)
                                    {
                                        SwapWire(wireGameObject1, wireGameObject2, false);
                                    }
                                    else // for every thing else:
                                    {
                                        CancelThisMouseAction(_currentMouseAction);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                CancelThisMouseAction(_currentMouseAction); return;
                            }
                        }
                        break;
                    case MouseAction.MoveCam:
                        CancelThisMouseAction(_currentMouseAction);
                        break;
                    case MouseAction.ClickOnVirutalButton:
                        CancelThisMouseAction(_currentMouseAction);
                        break;

                }
            }
        }
    }
    private GameObject RayCastAndGetToppestGameobject(Vector3 currentMouseWorldPos)
    {
        Vector2 mousePostion = currentMouseWorldPos;
        Collider2D[] colliders = Physics2D.OverlapPointAll(mousePostion, layerMask);
        if (colliders.Length == 0) return null;

        int maxSortingOrder = 0;
        Collider2D toppestCollider = null;
        foreach (Collider2D collider in colliders)
        {
            SpriteRenderer sr = collider.gameObject.GetComponent<SpriteRenderer>();
            int currentSRSO = sr.sortingOrder;
            if (currentSRSO >= maxSortingOrder)
            {
                maxSortingOrder = currentSRSO;
                toppestCollider = collider;
            }
        }
        return toppestCollider.gameObject;
    }
    private void SetMouseAction(MouseAction newMouseAction)
    {
        _currentMouseAction = newMouseAction;
        switch (_currentMouseAction)
        {
            case MouseAction.None:
                _cursorManager.SetToDefaultCursor();
                break;
            case MouseAction.MoveObject:
                _cursorManager.SetToGrabbedHandCursor();
                break;
            case MouseAction.PlaceWire:
                _cursorManager.SetToHandCursor();
                break;
            case MouseAction.MoveWire:
                _cursorManager.SetToHandCursor();
                break;
            case MouseAction.MoveCam:
                _cursorManager.SetToMoveCameraCursor();
                break;
            case MouseAction.ClickOnVirutalButton:
                _cursorManager.SetToHandCursor();
                break;
            default:
                _cursorManager.SetToDefaultCursor();
                break;
        }
        //   Debug.Log($"Current Mouse Action : {_currentMouseAction}");
    }
    private void CancelThisMouseAction(MouseAction mouseAction)
    {
        switch (mouseAction)
        {
            case MouseAction.None:
                _cursorManager.SetToDefaultCursor();
                break;
            case MouseAction.MoveObject:
                {
                    _objectToGridPosition.CheckAndMoveObejectToward(objectToMove.transform);

                    // return back sorting order to previus:
                    ShowObjectInBehinde();

                    objectToMove = null;
                    objectToMoveBorder = null;
                    SetMouseAction(MouseAction.None);
                    _cursorManager.SetToDefaultCursor();
                }
                break;
            case MouseAction.PlaceWire:
                {
                    _wireStartPoint = null;
                    _wireEndPoint = null;
                    _currentTraitOfSellectedWirePoint = Trait.None;
                    _previewWireObject.SetActive(false);

                    SetMouseAction(MouseAction.None);
                    _cursorManager.SetToDefaultCursor();
                }
                break;
            case MouseAction.MoveWire:
                {
                    Destroy(_sellectedWireToMoveGameObject);
                    _sellectedWireToMoveGameObject = null;
                    _wireEndPoint.GetComponent<IWirable>().Wire_GameObject = null;
                    _wireStartPoint.GetComponent<IWirable>().Wire_GameObject = null;

                    _wireStartPoint = null;
                    _wireEndPoint = null;
                    _currentTraitOfSellectedWirePoint = Trait.None;
                    _previewWireObject.SetActive(false);

                    SetMouseAction(MouseAction.None);
                    _cursorManager.SetToDefaultCursor();
                }
                break;
            case MouseAction.MoveCam:
                {
                    SetMouseAction(MouseAction.None);
                    _cursorManager.SetToDefaultCursor();
                }
                break;
            case MouseAction.ClickOnVirutalButton:
                {
                    SetMouseAction(MouseAction.None);
                    _cursorManager.SetToDefaultCursor();
                }
                break;
        }
    }
    private void SpawnWire(GameObject start, GameObject end)
    {
        ItemManager.Singeleton.AddWire(start, end);

        _wireStartPoint = null;
        _wireEndPoint = null;
        _currentTraitOfSellectedWirePoint = Trait.None;
        _previewWireObject.SetActive(false);

        SetMouseAction(MouseAction.None);
    }
    private void MoveWire()
    {
        _sellectedWireToMoveGameObject.SetActive(true);
        Wire currentWire = _sellectedWireToMoveGameObject.GetComponent<Wire>();
        currentWire.StartPoint = _wireStartPoint;
        currentWire.EndPoint = _wireEndPoint;
        _wireEndPoint.GetComponent<IWirable>().Wire_GameObject = _sellectedWireToMoveGameObject;
        _wireStartPoint.GetComponent<IWirable>().Wire_GameObject = _sellectedWireToMoveGameObject;
       
        _sellectedWireToMoveGameObject = null;
        _wireStartPoint = null;
        _wireEndPoint = null;
        _currentTraitOfSellectedWirePoint = Trait.None;
        _previewWireObject.SetActive(false);

        SetMouseAction(MouseAction.None);
    }
    private void SpawnPreviewWire()
    {
        _previewWireObject = Instantiate(_previewWirePrefab, Vector3.zero, Quaternion.identity);
        _previewWireObject.name = "PreviewWire";
        _previewWireObject.transform.parent = transform;
        _previewWireComponent = _previewWireObject.GetComponent<PreviewWire>();

        _previewWireObject.SetActive(false);
    }
    private void MoveCameraOnScreenEdge()
    {
        if (IsPointerOnScreenEdge.Top())
        {
            float thisFrameMovement = Time.deltaTime * camMoveSpeedOnScreenEdge * Camera.main.orthographicSize;
            Vector3 amountToMove = new Vector3(0, thisFrameMovement, 0);
            cam.transform.position += amountToMove;
            Border.CheckAndResolve_BorderInBackGroundBorder(camBorder);
        }
        else if (IsPointerOnScreenEdge.Down())
        {
            float thisFrameMovement = Time.deltaTime * camMoveSpeedOnScreenEdge * Camera.main.orthographicSize;
            Vector3 amountToMove = new Vector3(0, -thisFrameMovement, 0);
            cam.transform.position += amountToMove;
            Border.CheckAndResolve_BorderInBackGroundBorder(camBorder);
        }

        if (IsPointerOnScreenEdge.Right())
        {
            float thisFrameMovement = Time.deltaTime * camMoveSpeedOnScreenEdge * Camera.main.orthographicSize;
            Vector3 amountToMove = new Vector3(thisFrameMovement, 0, 0);
            cam.transform.position += amountToMove;
            Border.CheckAndResolve_BorderInBackGroundBorder(camBorder);
        }
        else if (IsPointerOnScreenEdge.Left())
        {
            float thisFrameMovement = Time.deltaTime * camMoveSpeedOnScreenEdge * Camera.main.orthographicSize;
            Vector3 amountToMove = new Vector3(-thisFrameMovement, 0, 0);
            cam.transform.position += amountToMove;
            Border.CheckAndResolve_BorderInBackGroundBorder(camBorder);
        }
    }
    private void SwapWire(GameObject wireGameObject1, GameObject wireGameObject2, bool canSawpStartPoint)
    {
        wireGameObject1.SetActive(true);
        wireGameObject2.SetActive(true);

        Wire wire1 = wireGameObject1.GetComponent<Wire>();
        Wire wire2 = wireGameObject2.GetComponent<Wire>();
        GameObject tempPoint = null;

        // swaping StartPoint:
        if (canSawpStartPoint)
        {
            tempPoint = wire1.StartPoint;
            wire1.StartPoint = wire2.StartPoint;
            wire2.StartPoint = tempPoint;

            IWirable wire1_Wirable = wire1.StartPoint.GetComponent<IWirable>();
            IWirable wire2_Wirable = wire2.StartPoint.GetComponent<IWirable>();

            wire1_Wirable.Wire_GameObject = wireGameObject1;
            wire2_Wirable.Wire_GameObject = wireGameObject2;
        }
        // swaping EndPoint:
        else
        {
            tempPoint = wire1.EndPoint;
            wire1.EndPoint = wire2.EndPoint;
            wire2.EndPoint = tempPoint;

            IWirable wire1_Wirable = wire1.EndPoint.GetComponent<IWirable>();
            IWirable wire2_Wirable = wire2.EndPoint.GetComponent<IWirable>();

            wire1_Wirable.Wire_GameObject = wireGameObject1;
            wire2_Wirable.Wire_GameObject = wireGameObject2;
        }
       
        _sellectedWireToMoveGameObject = null; // null this pointer to preventing to got destroyed on CancelThisMouseAction
        _wireStartPoint = null;
        _wireEndPoint = null;
        _currentTraitOfSellectedWirePoint = Trait.None;
        _previewWireObject.SetActive(false);

        SetMouseAction(MouseAction.None);
    }
    private void ShowObjectInFront(GameObject go)
    {
        SortingGroup sg = go.GetComponent<SortingGroup>();
        if (sg == null) { Debug.LogError($"Gameobject( {go.name} ) dont have any SortingGroup Component!"); }
        else
        {
            // if object contain any canvas then show them in front:
            Canvas canvas = go.GetComponentInChildren<Canvas>();
            if(canvas != null) canvas.sortingOrder += objectToMoveSortingOrderMoveOffset;
            
            objectToMoveSortingGroup = sg;
            objectToMoveSortingGroup.sortingOrder += objectToMoveSortingOrderMoveOffset;
        }
    }
    private void ShowObjectInBehinde()
    {
        // return back sorting order to previus:
        if (objectToMoveSortingGroup != null)
        {
            // if object contain any canvas then show them in front:
            Canvas canvas = objectToMoveSortingGroup.GetComponentInChildren<Canvas>();
            if (canvas != null) canvas.sortingOrder -= objectToMoveSortingOrderMoveOffset;

            objectToMoveSortingGroup.sortingOrder -= objectToMoveSortingOrderMoveOffset;
            objectToMoveSortingGroup = null;
        }
    }


    // internal mouse action:---------------------------------------------------------------------------------
    public enum MouseAction
    {
        None,
        MoveObject,
        PlaceWire,
        MoveWire,
        MoveCam,
        ClickOnVirutalButton,
    }

} // end of class