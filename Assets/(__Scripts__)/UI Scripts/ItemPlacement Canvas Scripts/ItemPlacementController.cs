using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPlacementController : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject UiItemPrefab;    
    [SerializeField] GameObject itemPlacementCursorPrefab;
    private GameObject itemPlacementCursor;
   // private GameObject currentPrefab = null;

    // Unity Components: ///////////////////////////////////////////////////////
    private Camera cam;
    private Item currentItem;
    ObjectToGridPosition objectToGridPosition;
    [SerializeField] ItemPlacementNotificationControllder itemPlacementNotificationControllder;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    private bool isTouch0OverUI_lastFrame;
    private Vector3 currentMousePosition;

    private float itemPlacementCursor_firstScale;
    [SerializeField] float itemPlacementCursor_touchScaleMul = 2;
    [SerializeField] float itemPlacementCursor_mouseScaleMul = 1;
    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        cam = Camera.main;
    }
    private void Start()
    {   
        objectToGridPosition = GameManager.Instance.GetObjectToGridPositionScript();
        itemPlacementCursor = Instantiate(itemPlacementCursorPrefab, transform, true);
        itemPlacementCursor.transform.position = Vector3.zero;
        itemPlacementCursor.SetActive(false);
        itemPlacementCursor_firstScale = itemPlacementCursor.transform.localScale.x;
        itemPlacementNotificationControllder.Hide();

    }
    private void Update()
    {
        currentMousePosition = cam.ScreenToWorldPoint(Input.mousePosition); currentMousePosition.z = 0;

        // update itemPlacementCursor position every frame:
        itemPlacementCursor.transform.position = currentMousePosition;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            // update itemPlacementCursor position every frame:
            itemPlacementCursor.transform.position = currentMousePosition;

            // check for cancelation with right click;
            if (Input.GetKeyDown(KeyCode.Mouse1)) { CancelAction(); }
        }

        //TouchInput();
        //MouseInput();
        Set_isTouch0OverUI_lastFrame();
    }


    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void SetCurrentItem(Item newItem)
    {
        currentItem = newItem;
    }

    public void OnBeginDragOnUIItem()
    {
        itemPlacementCursor.transform.position = currentMousePosition;
        itemPlacementCursor.SetActive(true);
        SetScale_itemPlacementCursor(itemPlacementCursor_mouseScaleMul);

        itemPlacementNotificationControllder.Init(currentItem);
        itemPlacementNotificationControllder.Show();
    }
    public void OnEndDragOnUIItem()
    {
        if (EventSystem.current.IsPointerOverGameObject(MouseInputAction.EVENT_SYSTEM_MOUSE_ID) 
            || isTouch0OverUI_lastFrame
            || !Border.IsThePointInsideBackGroundBorder(currentMousePosition))
        {
            CancelAction();
        }
        else
        {
            if (currentItem == null) return;

            PlacePrefabInstanceAtScreenPosition(Input.mousePosition);
            itemPlacementCursor.SetActive(false);
            itemPlacementNotificationControllder.Hide();
        }
    }

    // C# Private Methods: /////////////////////////////////////////////////////    
    private void PlacePrefabInstanceAtScreenPosition(Vector3 screenPos)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos); worldPos.z = 0;
        int item_id = currentItem.Id;
        GameObject spawnedObject = ItemManager.Singeleton.AddItemAndReturnGameObject(item_id, worldPos, Item_Rotation.n);
        objectToGridPosition.CheckAndMoveObejectToward(spawnedObject.transform);

        currentItem = null;
    }
    private void CancelAction()
    {
        currentItem = null;
        itemPlacementCursor.SetActive(false);
        itemPlacementNotificationControllder.Hide();
    }
    private void TouchInput()
    {
        if (currentItem != null)
        {
            if (Input.touchCount > 0)
            {
                Touch touch0 = Input.GetTouch(0);
                Vector3 currentTouchPosition = cam.ScreenToWorldPoint(Input.mousePosition); currentTouchPosition.z = 0;

                if (touch0.phase == TouchPhase.Began)
                {
                    itemPlacementCursor.SetActive(true);
                    itemPlacementCursor.transform.position = currentTouchPosition;
                    SetScale_itemPlacementCursor(itemPlacementCursor_touchScaleMul);
                }
                if (touch0.phase == TouchPhase.Moved)
                {
                    // update itemPlacementCursor position every time touch has moved:
                    itemPlacementCursor.transform.position = currentTouchPosition;
                }
                if (touch0.phase == TouchPhase.Ended)
                {
                    if (isTouch0OverUI_lastFrame || (!Border.IsThePointInsideBackGroundBorder(currentTouchPosition)))
                    {
                        CancelAction();
                    }
                    else
                    {
                        PlacePrefabInstanceAtScreenPosition(touch0.position);
                    }
                }
            }
        }
    }
    private void MouseInput()
    {
        // if any touch input present, then return:
        if (Input.touchCount > 0) return;


        if (currentItem != null)
        {
            Vector3 currentMousePosition = cam.ScreenToWorldPoint(Input.mousePosition); currentMousePosition.z = 0;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                itemPlacementCursor.SetActive(true);
                itemPlacementCursor.transform.position = currentMousePosition;
                SetScale_itemPlacementCursor(itemPlacementCursor_mouseScaleMul);
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                // update itemPlacementCursor position every frame:
                itemPlacementCursor.transform.position = currentMousePosition;

                // check for cancelation with right click;
                if (Input.GetKeyDown(KeyCode.Mouse1)) { CancelAction(); return; }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (EventSystem.current.IsPointerOverGameObject(MouseInputAction.EVENT_SYSTEM_MOUSE_ID)
                    || !Border.IsThePointInsideBackGroundBorder(currentMousePosition))
                {
                    CancelAction();
                }
                else
                {
                    PlacePrefabInstanceAtScreenPosition(Input.mousePosition);
                }
            }

        }
    }

    private void Set_isTouch0OverUI_lastFrame()
    {
        if (Input.touchCount > 0)
        {
            isTouch0OverUI_lastFrame = EventSystem.current.IsPointerOverGameObject(0);
        }
        else
        {
            isTouch0OverUI_lastFrame = false;
        }
    }

    private void SetScale_itemPlacementCursor(float mul)
    {
        float newScale = itemPlacementCursor_firstScale * mul;
        itemPlacementCursor.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
} // end of class