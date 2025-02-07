using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCursorControlller : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] bool rotateRight = true;
    [SerializeField] float rotateSpeed = 1;
    private bool canRotate = false;
    private Animator _animator;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RotateCursor();
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    private void OnEnable()
    {
        canRotate = true;
        _animator.enabled = true;
    }
    private void OnDisable()
    {
        canRotate = false;
        _animator.enabled = false;
    }

    // C# Public Methods: //////////////////////////////////////////////////////
    // C# Private Methods: /////////////////////////////////////////////////////
    private void RotateCursor()
    {
        if (!canRotate) return;

        transform.Rotate(Vector3.forward, Time.deltaTime * rotateSpeed * (rotateRight ? 1 : -1), Space.World);
    }

} // end of class