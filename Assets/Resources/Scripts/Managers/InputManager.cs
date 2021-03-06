﻿
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Directions")]
    public float _horizontal;
    public float _vertical;
    public Vector3 _direction;
    public Quaternion _rotation;

    [Header("Mouse Input")]
    public float _mouseSensitivity = 100f;
    public float _mouseX;
    public float _mouseY;
    public float _xRotation = 0f;
    public Vector2 _rotationClamp;
    public Transform _cameraPivot;
    [Tooltip("Z - lock, X - unlock")]
    public bool _lockCursor = false;

    [Header("Has Input")]
    public bool _hasMovementInput;
    public bool _hasMouseInput;
    public bool _hasAimInput;
    public bool _hasRotationInput;

    [Header("Camera Raycast")]
    public LayerMask _rayMask;
    public Camera _camera;
    public Transform _camTransform;
    public Transform _mousePickTransform;
    public float _rayLength;
    public Vector3 _mousePickPoint;

    private void Start()
    {
        ChangeCursorLock(_lockCursor);
    }


    private void Update()
    {
        UserMovementInput();
        TopDownMouseInput();
    }


    public void UserMovementInput()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(_horizontal) > 0 || Mathf.Abs(_vertical) > 0)
        {
            if (_hasMovementInput == false)
                _hasMovementInput = true;

            _direction = new Vector3(_horizontal, 0, _vertical).normalized;

            if (_direction != Vector3.zero)
                _rotation = Quaternion.LookRotation(_direction);
        }

        else
        {
            if (_hasMovementInput == true)
                _hasMovementInput = false;
        }
    }

    #region Mouse Input
    public void UserMouseLookInput()
    {
        _mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        _mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        if (Mathf.Abs(_mouseX) > 0 || Mathf.Abs(_mouseY) > 0)
        {
            if (_hasMouseInput == false)
                _hasMouseInput = true;

            _xRotation -= _mouseY;
            _xRotation = Mathf.Clamp(_xRotation, _rotationClamp.x, _rotationClamp.y);
        }

        else
        {
            if (_hasMouseInput)
                _hasMouseInput = false;
        }

        _hasAimInput = Input.GetMouseButton(0);
        _hasRotationInput = Input.GetMouseButton(1);

        if (Input.GetKeyDown(KeyCode.Z))
            ChangeCursorLock(true);
        if (Input.GetKeyDown(KeyCode.X))
            ChangeCursorLock(false);
    }


    public void TopDownMouseInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            ChangeCursorLock(true);
        if (Input.GetKeyDown(KeyCode.X))
            ChangeCursorLock(false);

        _mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
        _mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;

        _hasAimInput = Input.GetMouseButton(0);
        _hasRotationInput = Input.GetMouseButton(1);

        RaycastHit _hit;
        Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit, _rayLength, _rayMask))
        {
            _mousePickPoint = _hit.point;
            _mousePickTransform.position = _mousePickPoint;
        }
    }

    #endregion


    public void ChangeCursorLock(bool val)
    {
        if (val == true)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    public void CameraRaycast()
    {
        RaycastHit _hit;

        if (Physics.Raycast(_camTransform.position, _camTransform.forward, out _hit, _rayLength, _rayMask))
        {
            _mousePickPoint = _hit.point;
            _mousePickTransform.position = _mousePickPoint;
        }
    }
}