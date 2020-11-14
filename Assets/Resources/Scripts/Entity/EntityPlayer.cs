
using UnityEngine;

public class EntityPlayer : Entity
{
    [Header("Sport Collisions")]
    public LayerMask _mask;
    public Transform _colliderPos;
    public float _collisionRadius = 1f;

    [Header("Player Sport Shoot Cooldowns")]
    [Tooltip("0-basket, 1-football, 2-baseball")]
    public float[] _sportShootCooldowns;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        PlayerShoot();
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
                PlayerJump();

            if (_isGrounded == false)
                PlayAnimation(3);

            else if (InputManager.Instance._hasAimInput == false && _isShooting == false && _isGrounded)
            {
                if (InputManager.Instance._hasMovementInput)
                    PlayAnimation(1);
                else
                    PlayAnimation(0);
            }
        }
    }


    void PlayerJump()
    {
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }


    public void PlayerShoot()
    {
        if (Input.GetMouseButtonUp(0) && _ballLauncher._hasBall && _isShooting == false)
        {
            if (_entitySport == IEntity.entitySport.basketball)
            {
                StartCoroutine(ShootCooldown(_sportShootCooldowns[0]));
                PlayShootAnimation(0);
            }

            if (_entitySport == IEntity.entitySport.football)
            {
                StartCoroutine(ShootCooldown(_sportShootCooldowns[1]));
                PlayShootAnimation(1);
            }

            if (_entitySport == IEntity.entitySport.baseball)
            {
                StartCoroutine(ShootCooldown(_sportShootCooldowns[2]));
                PlayShootAnimation(2);
            }
        }
    }

    public void PlayerSportCollisions()
    {
        if (_colliderPos != null)
        {
            var _colliders = Physics.OverlapSphere(_colliderPos.position, _collisionRadius);

            if (_colliders != null && _colliders.Length > 0)
            {

            }
        }
    }

    new private void OnDrawGizmos()
    {
        if (_colliderPos != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_colliderPos.position, _collisionRadius);
        }
    }
}