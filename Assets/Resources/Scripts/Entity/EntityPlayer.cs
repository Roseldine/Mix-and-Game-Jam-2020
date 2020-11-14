
using UnityEngine;

public class EntityPlayer : Entity
{
    [Header("Player Dependencies")]
    public Transform _graphicContainer;
    public Transform _cameraPivotX;
    public Transform _cameraPivotY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        PlayerMovement();
    }



    public void PlayerMovement()
    {
        if (_entityType == IEntity.entityType.player)
        {
            if (InputManager.Instance._hasMouseInput)
            {
                _cameraPivotX.Rotate(Vector3.up * InputManager.Instance._mouseX);
                _cameraPivotY.Rotate(Vector3.right * InputManager.Instance._mouseY * -1);
            }

            if (InputManager.Instance._hasMovementInput)
            {
                _rigidbody.velocity = _cameraPivotX.rotation * InputManager.Instance._direction * _moveSpeed;
            }

            else
            {
                _rigidbody.velocity = Vector3.zero;
            }

            if (InputManager.Instance._hasAimInput == false)
                SmoothLookAt(_graphicContainer, _rigidbody.velocity, _lookSpeed);
            else
                SmoothLookAt(_graphicContainer, _cameraPivotX.rotation, _lookSpeed);


            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded == true)
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
}