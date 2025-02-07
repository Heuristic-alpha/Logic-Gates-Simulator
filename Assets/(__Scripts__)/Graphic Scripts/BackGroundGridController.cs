using UnityEngine;

public class BackGroundGridController : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    private Camera _mainCam;
    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] private Material _gridMat;
    [SerializeField] Color _defaultBackgroundBaseColor;
    [SerializeField] Color _defaultBackgroundLineColor;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _mainCam = Camera.main;
        Set_BackColor(_defaultBackgroundBaseColor);
        Set_LineColor(_defaultBackgroundLineColor);
    }

    private void LateUpdate()
    {
        FollowCameraAndResize();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////

    public void Set_LineColor(Color color) => _gridMat.SetColor("_Color1", color);
    public Color Get_LineColor() => _gridMat.GetColor("_Color1");
    public void Set_BackColor(Color color) => _gridMat.SetColor("_Color2", color);
    public Color Get_BackColor() => _gridMat.GetColor("_Color2");
    public void Set_CellSize(float size) => _gridMat.SetFloat("_Cell_Size", size);
    public float Get_CellSize() => _gridMat.GetFloat("_Cell_Size");

    // C# Private Methods: /////////////////////////////////////////////////////
    private void FollowCameraAndResize()
    {
        float cameraHeight = _mainCam.orthographicSize * 2;
        float cameraWidth = cameraHeight * _mainCam.aspect;
        // set size:
        transform.localScale = new Vector3(cameraWidth, cameraHeight, 1);
        // set position:
        transform.position = new Vector3(_mainCam.transform.position.x, _mainCam.transform.position.y, transform.position.z);
    }

} // end of class