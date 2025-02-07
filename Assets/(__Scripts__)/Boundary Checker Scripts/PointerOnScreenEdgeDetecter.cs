using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerOnScreenEdgeDetecter : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    Camera cam;
    [SerializeField] int _pixelDistanceFromEdges = 100;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        CheckForPointerNearEdgeScreen();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////

    public void CheckForPointerNearEdgeScreen()
    {
        // check near screen top:
        if (Input.mousePosition.y > Screen.height - _pixelDistanceFromEdges) IsPointerOnScreenEdge.SetTopValue(true);
        else IsPointerOnScreenEdge.SetTopValue(false);

        // check near screen down:
        if (Input.mousePosition.y <  _pixelDistanceFromEdges) IsPointerOnScreenEdge.SetDownValue(true);
        else IsPointerOnScreenEdge.SetDownValue(false);

        // check near screen right:
        if (Input.mousePosition.x > Screen.width - _pixelDistanceFromEdges) IsPointerOnScreenEdge.SetRightValue(true);
        else IsPointerOnScreenEdge.SetRightValue(false);

        // check near screen left:
        if (Input.mousePosition.x < _pixelDistanceFromEdges) IsPointerOnScreenEdge.SetLeftValue(true);
        else IsPointerOnScreenEdge.SetLeftValue(false);
      
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void ShowValueOnDebug()
    {
        Debug.Log($"**** ScreenBase top={IsPointerOnScreenEdge.Top()} down={IsPointerOnScreenEdge.Down()} right={IsPointerOnScreenEdge.Right()} left={IsPointerOnScreenEdge.Left()}");     
    }

} // end of class

public static class IsPointerOnScreenEdge
{
    private static bool _topValue = false;
    private static bool _downValue = false;
    private static bool _rightValue = false;
    private static bool _leftValue = false;
    public static void SetTopValue(bool value) { _topValue = value; }
    public static void SetDownValue(bool value) { _downValue = value; }
    public static void SetRightValue(bool value) { _rightValue = value; }
    public static void SetLeftValue(bool value) { _leftValue = value; }


    public static bool Top() { return _topValue; }
    public static bool Down() { return _downValue; }
    public static bool Right() { return _rightValue; }
    public static bool Left() { return _leftValue; }

}