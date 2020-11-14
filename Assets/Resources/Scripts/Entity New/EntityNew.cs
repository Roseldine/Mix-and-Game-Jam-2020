
using System.Collections;
using UnityEngine;

public class EntityNew : MonoBehaviour
{
    [Header("Entity Build")]
    public IEntity.entityType _entityType;
    public IEntity.entitySport _entitySport;

    [Header("Dependencies")]
    public Rigidbody _rigidbody;
    public UnityEngine.AI.NavMeshAgent _agent;
    public Transform _entityTransform;
    public EntityNewStateMachine _stateMachine;
    public EntityAnimation _animation;

    [Header("Health")]
    public int _maxHealth = 3;
    public int _health;
    public bool _isDead;

    [Header("Movement")]
    public float _moveSpeed = 7f;
    public float _lookSpeed = 10f;
    public bool _canMove = true;

    [Header("Attack")]
    [Tooltip("0-basket, 1-footbal, 2-baseball")]
    public Transform[] _ballSpawns;
    [Tooltip("0-basket, 1-footbal, 2-baseball")]
    public GameObject[] _ballPrefabs;
    [Tooltip("0-basket, 1-footbal, 2-baseball")]
    public float[] _shootCooldowns;
    public bool _canShoot = true;

    [Header("Graphics")]
    public Transform _graphicContainer;
    public Transform _graphic;
    public Animator _animator;

    [Header("Player Camera")]
    public Transform _cameraPivot;
    public float _camSensitivity;
    [Range(0, 20)] public float _camFollowSpeed = 20f;
    [Range(0, 20)] public float _camAdvanceDistance = 5f;


    #region Initialization

    private void Start()
    {
        StartVariables();
    }


    public void StartVariables()
    {
        foreach (Transform t in _graphicContainer)
            if (t.tag == "Graphic")
            {
                _graphic = t;
                _animator = _graphic.GetComponent<Animator>();
            }

        _animation._animator = _animator;

        _canMove = true;
        _canShoot = true;
    }

    #endregion


    private void Update()
    {
        UpdateEntity();
    }

    public void UpdateEntity()
    {
        CameraFollowPlayer();
        PlayerInput();
        _stateMachine.UpdateStateMachine();
    }

    #region Player Methods
    public void PlayerMovement()
    {
        if (_entityType == IEntity.entityType.player)
        {
            if (InputManager.Instance._hasMovementInput)
            {
                _rigidbody.velocity = _cameraPivot.localRotation * InputManager.Instance._direction * _moveSpeed;
                SmoothLookAt(_graphicContainer, _rigidbody.velocity, _lookSpeed);
            }

            else
            {
                _rigidbody.velocity = Vector3.zero;
            }
        }
    }

    public void PlayerInput()
    {
        if (_entityType == IEntity.entityType.player)
        {
            if (Input.GetMouseButtonDown(0) && _canShoot)
            {
                if ((_stateMachine._currentState as EntityState)._stateId != 3)
                    _stateMachine.ChangeEntityState(_stateMachine._states[3]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
                SetSport(0);

            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetSport(1);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                SetSport(2);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                SetSport(3);

            if (InputManager.Instance._hasRotationInput && Mathf.Abs(InputManager.Instance._mouseX) > 0)
            {
                _cameraPivot.Rotate(Vector3.up * InputManager.Instance._mouseX * _camSensitivity);
            }
        }
    }


    void CameraFollowPlayer()
    {
        if (_entityType == IEntity.entityType.player)
        {
            _cameraPivot.position = _entityTransform.position;

            //var _dir = _entityTransform.position - _cameraPivot.position;
            //var _targetDir = _cameraPivot.forward + _dir;
            //_cameraPivot.Translate(_targetDir * _camFollowSpeed * Time.deltaTime);
            //var _targetDir = _entityTransform.position + _dir;
            //_cameraPivot.Translate(_targetDir * _camFollowSpeed * Time.deltaTime);
        }
    }


    public void PlayerShoot()
    {
        if (Input.GetMouseButtonUp(0) && _canShoot)
        {
            Shoot();
        }
    }
    #endregion // player

    #region Shoot

    public void Shoot()
    {
        int _ballId = (int)_entitySport - 1;
        var _ball = Instantiate(_ballPrefabs[_ballId], _ballSpawns[_ballId]);
        StartCoroutine(ShootCooldown(_shootCooldowns[_ballId]));

        if (_entitySport == IEntity.entitySport.basketball)
        {
            
        }

        else if (_entitySport == IEntity.entitySport.football)
        {

        }

        else if (_entitySport == IEntity.entitySport.baseball)
        {

        }
    }

    IEnumerator ShootCooldown(float cooldown)
    {
        _canShoot = false;
        yield return new WaitForSeconds(cooldown);
        _canShoot = true;
    }

    #endregion // shoot


    #region Utility

    /// <summary>
    /// 0-none, 1-basket, 2-football, 3-baseball
    /// </summary>
    public void SetSport(int id)
    {
        _entitySport = (IEntity.entitySport)id;
    }


    public void SmoothLookAt(Transform rotator, Vector3 direction, float speed)
    {
        if (direction != Vector3.zero)
        {
            rotator.rotation = Quaternion.Slerp(rotator.rotation, Quaternion.LookRotation(direction), speed * Time.deltaTime);
        }
    }

    public void SmoothLookAt(Transform rotator, Quaternion rotation, float speed)
    {
        rotator.rotation = Quaternion.Slerp(rotator.rotation, rotation, speed * Time.deltaTime);
    }


    #endregion
}
