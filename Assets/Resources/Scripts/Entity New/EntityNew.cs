
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

    [Header("Shoot Forces")]
    public float _basketForce;
    public float _footballForce;
    public float _baseballForce;

    [Header("Line")]
    public LineRenderer _line;
    public LineRenderer _lineTest;
    public float _straightLineLength = 4f;
    public float _straightLineHeight = .25f;

    [Header("Basketball Path Prediction")]
    public AnimationCurve _basketCurve;
    public float _basketCurveHeightMultiplier = 2f;
    public int _basketArcIterations;
    public float _basketArcMinHeight = 2f;
    public float _basketArcMaxHeight = 5f;
    public float _basketArcMaxDistance = 35f;
    public float _basketTimeToDestination = 2f;
    public bool _invert;
    Vector3[] _path;

    [Header("Baseball Path Prediction")]


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

        _line.useWorldSpace = true;
        _lineTest.useWorldSpace = true;
        SetSport((int)_entitySport);
    }

    #endregion


    private void Update()
    {
        UpdateEntity();
    }

    public void UpdateEntity()
    {
        if (_entityType == IEntity.entityType.player)
        {
            PlayerAim();
            CameraFollowPlayer();
            PlayerInput();
        }

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

    void PlayerAim()
    {
        var _aimDirection = (InputManager.Instance._mousePickTransform.position - _entityTransform.position).normalized;

        var _ballSpawn = _ballSpawns[(int)_entitySport - 1];
        var _initPos = _ballSpawn.position;
        var _relativeDirection = _initPos + _aimDirection;

        if (_entitySport == IEntity.entitySport.football || _entitySport == IEntity.entitySport.baseball)
        {
            var _height = Vector3.up * _straightLineHeight;
            _line.SetPosition(0, _height + _initPos);
            _line.SetPosition(1, _height + _relativeDirection * _straightLineLength);
        }

        else if (_entitySport == IEntity.entitySport.basketball)
        {
            _lineTest.SetPosition(0, _ballSpawn.position);
            _lineTest.SetPosition(1, InputManager.Instance._mousePickTransform.position);

            var _dir = (_lineTest.GetPosition(1) - _lineTest.GetPosition(0)).normalized;
            var _distance = Vector3.Distance(_lineTest.GetPosition(0), _lineTest.GetPosition(1));
            var _factor = _distance / _basketArcIterations;

            if (_invert == false)
                _basketCurveHeightMultiplier = _distance * _basketArcMaxHeight / _basketArcMaxDistance;
            else
                _basketCurveHeightMultiplier = _basketArcMaxHeight - (_distance * _basketArcMaxHeight / _basketArcMaxDistance);

            _basketCurveHeightMultiplier = Mathf.Clamp(_basketCurveHeightMultiplier, _basketArcMinHeight, _basketArcMaxHeight);

            _line.SetPosition(0, _lineTest.GetPosition(0));
            for (int i = 1; i < _basketArcIterations; i++)
            {
                var _pos = _lineTest.GetPosition(0) + _dir * (_factor * i);
                _path[i] = _pos;
                _line.SetPosition(i, _pos + Vector3.up * _basketCurve.Evaluate(_factor * i / _distance) * _basketCurveHeightMultiplier);
            }

            if (Input.GetMouseButtonDown(0))
            {
                int _ballId = (int)_entitySport - 1;
                var _ball = Instantiate(_ballPrefabs[_ballId], _ballSpawns[_ballId].position, Quaternion.identity);
                StartCoroutine(ShootBasketBall(_ball, _distance, _lineTest.GetPosition(1)));
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

    IEnumerator ShootBasketBall(GameObject ball, float distance, Vector3 destination)
    {
        float t = 0;
        float _duration = distance * _basketTimeToDestination / _basketArcMaxDistance;
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        var _localPath = _path;
        Vector3 _initPos = ball.transform.position;
        Vector3 _pos = _initPos;
        float _multiplier = _basketCurveHeightMultiplier;

        while (t < _duration)
        {
            _pos = Vector3.Lerp(_initPos, destination, t / _duration);
            _pos.y = _basketCurve.Evaluate(Vector3.Distance(_pos, destination) / distance) * _multiplier;
            ball.transform.position = _pos;
            //rb.velocity = _pos;

            t += Time.deltaTime;
            yield return null;
        }
    }

    #endregion // shoot


    #region Utility

    /// <summary>
    /// 0-none, 1-basket, 2-football, 3-baseball
    /// </summary>
    public void SetSport(int id)
    {
        _entitySport = (IEntity.entitySport)id;

        if (_entitySport == IEntity.entitySport.none)
            _line.positionCount = 0;

        else if (_entitySport == IEntity.entitySport.basketball)
        {
            _path = new Vector3[_basketArcIterations];
            _line.positionCount = _basketArcIterations;
        }

        else if (_entitySport == IEntity.entitySport.football)
            _line.positionCount = 2;

        else if (_entitySport == IEntity.entitySport.baseball)
            _line.positionCount = 2;
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
