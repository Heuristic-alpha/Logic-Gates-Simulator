using UnityEngine;
using UnityEngine.EventSystems;


public class ContextMenuManager : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////  
    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    private Camera cam;
    private int layerMask;

    private float pressTime;
    private float pressThreshold = 1f;// 1 second

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        cam = Camera.main;
        layerMask = 1 << LayerMask.NameToLayer("Logic");    
    }
    private void Update()
    {
        //////////////// Mouse GetKeyDown ///////////////////
        if (Input.GetKeyDown(KeyCode.Mouse1) && Input.touchCount==0)
        {
            OperationOnMouseKetKeyDown(Input.mousePosition);
        }

        TouchInput();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////    
    // C# Private Methods: /////////////////////////////////////////////////////
    private void OperationOnMouseKetKeyDown(Vector3 screenPos)
    {
        if (EventSystem.current.IsPointerOverGameObject(MouseInputAction.EVENT_SYSTEM_MOUSE_ID)
         || EventSystem.current.IsPointerOverGameObject(MouseInputAction.EVENT_SYSTEM_TOUCH_1_ID)) return;

        Vector3 pointerWorldPos = cam.ScreenToWorldPoint(screenPos); pointerWorldPos.z = 0;
        GameObject go = RayCastAndGetToppestGameobject(pointerWorldPos);
        if (go == null)
        {
            return;
        }
        else
        {
            TraitHolder traitHolder = go.GetComponent<TraitHolder>();
            if (traitHolder == null) { Debug.LogError($"Gameobject( {go.name} ) dont have any TraitHolder Component!"); return; }
            else
            {
                if (traitHolder.Contain(Trait.HaveContextMenu) && traitHolder.Contain(Trait.HaveBorder))
                {
                    ShowContextMenu(go,screenPos);
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
    private void ShowContextMenu(GameObject go, Vector3 screenPos)
    {
        ContextMenuScreen contextMenuScreen = UIManager.Singeleton.OpenAndReturnScreen<ContextMenuScreen>(HSCL.ScreenSample.ContextMenuScreen);
        contextMenuScreen.Init(go, screenPos, cam);     
    }
    private void TouchInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            switch(touch0.phase)
            { 
                case TouchPhase.Began:
                    pressTime = 0;
                    break;

                case TouchPhase.Stationary:
                    pressTime += Time.deltaTime;
                    if (pressTime > pressThreshold)
                    {
                        OperationOnMouseKetKeyDown(touch0.position);
                        pressTime = 0;
                    }
                    break;

                case TouchPhase.Ended:
                    pressTime = 0;
                    break;

                case TouchPhase.Canceled:
                    pressTime = 0;
                    break;
            }

        }
    }
   
} // end of class