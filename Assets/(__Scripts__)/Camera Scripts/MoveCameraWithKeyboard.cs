using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraWithKeyboard : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] float moveSpeed = 1;
    [SerializeField] CameraController cameraController;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void LateUpdate()
    {
        Vector3 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));
        if (direction.magnitude == 0) return;

        direction = direction.normalized;
        transform.position = transform.position + direction * Time.deltaTime * moveSpeed * cameraController.GetCameraSize();
        Border.CheckAndResolve_BorderInBackGroundBorder(cameraController.GetBorder());
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    // C# Private Methods: /////////////////////////////////////////////////////
}