
using UnityEngine;

public class EntityPlayer : Entity
{
    [Header("Sport Collisions")]
    public LayerMask _mask;
    public Transform _colliderPos;
    public float _collisionRadius;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        PlayerMovement();
        PlayerSportCollisions();
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

    public void PlayerSportCollisions()
    {
        var _colliders = Physics.OverlapSphere(_colliderPos.position, _collisionRadius);

        if (_colliders != null && _colliders.Length > 0)
        {

        }
    }
}