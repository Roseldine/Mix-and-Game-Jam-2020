
using System.Collections;
using UnityEngine;

public class EntityNew : MonoBehaviour
{
    [Header("Entity Build")]
    public IEntity.entityType _entityType;
    public IEntity.entitySport _entitySport;

    [Header("Dependencies")]
    public Rigidbody _rigidbody;
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
    public float _shootLookSpeed = 50f;
    public bool _canMove = true;
    float _internalMoveGravity;

    [Header("Ground Detection")]
    public Transform _groundDetector;
    public LayerMask _groundMask;
    [Range(0, .5f)] public float _detectorRadius = .15f;
    public bool _isGrounded;

    [Header("Attack")]
    [Tooltip("0-basket, 1-footbal, 2-baseball")]
    public Transform[] _ballSpawns;
    [Tooltip("0-basket, 1-footbal, 2-baseball")]
    public GameObject[] _ballPrefabs;
    [Tooltip("0-basket, 1-footbal, 2-baseball")]
    public float[] _shootCooldowns;
    public bool _canShoot = true;

    [Header("Enemy AI")]
    public UnityEngine.AI.NavMeshAgent _agent;
    public float _trophyDistanceThreshold;
    public float _agentShootCooldown;
    public bool _isAttacking;
    public bool _hasPath;

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

    [Header("Arc Commons")]
    public float _gravity = -25f;
    public int _resolution = 32;
    [Range(0, 2)] public float _heightMultiplier = .75f;

    [Header("Basketball Path Prediction")]
    public Rigidbody _basketSpawn;
    public float _basketHeight = 10f;
    [Range(0, 100)] public float _maxBasketHeight = 10f;

    [Header("Baseball Path Prediction")]
    public Rigidbody _baseballSpawn;
    public float _baseballHeight = 10f;
    [Range(0, 100)] public float _maxBaseballHeight = 2f;

    Vector3[] _path;
    Transform _target;

    [Header("Baseball Path Prediction")]


    [Header("Player Camera")]
    public Transform _cameraPivot;
    public Transform _camTargetMove;
    public Transform _camTargetIdle;
    [Range(0, 50)] public float _camMoveFollowSpeed = 20f;
    [Range(0, 50)] public float _camIdleFollowSpeed = 20f;
    public float _camSensitivity;


    AudioSource audioData;

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

        if (_entityType == IEntity.entityType.enemy)
        {
            if (_agent == null)
                _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            _agent.speed = _moveSpeed;
            _target = Trophy.Instance._enemyTarget;
        }
    }

    #endregion


    private void Update()
    {
        UpdateEntity();

        if (_isDead == true)
        {
            Debug.Log("Audio");
            audioData = GetComponent<AudioSource>();
            audioData.Play(0);
            Destroy(gameObject);
        }
    }

    public void UpdateEntity()
    {
        if (_entityType == IEntity.entityType.player)
        {
            PlayerAim();
            PlayerInput();
            
        }

        Gravity();
        _stateMachine.UpdateStateMachine();
    }

    #region Player Methods
    public void PlayerMovement()
    {
        if (_entityType == IEntity.entityType.player)
        {
            if (InputManager.Instance._hasMovementInput && _isGrounded)
            {
                var _velocity = _cameraPivot.localRotation * InputManager.Instance._direction * _moveSpeed;
                _velocity.y += _gravity * Time.deltaTime * _internalMoveGravity;
                _rigidbody.velocity = _velocity;

                SmoothLookAt(_graphicContainer, _cameraPivot.localRotation * InputManager.Instance._direction, _lookSpeed);
            }

            else
            {
                _rigidbody.AddForce(Vector3.up * _gravity);
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
        _target = InputManager.Instance._mousePickTransform;
        var _ballSpawn = _ballSpawns[(int)_entitySport - 1];

        if (_entitySport == IEntity.entitySport.football)
        {
            if (_line.positionCount != 2)
                _line.positionCount = 2;

            var _height = Vector3.up * _straightLineHeight;
            _lineTest.SetPosition(0, _ballSpawn.position);
            _lineTest.SetPosition(1, InputManager.Instance._mousePickTransform.position);

            var _dir = (_lineTest.GetPosition(1) - _lineTest.GetPosition(0)).normalized;
            _line.SetPosition(0, _lineTest.GetPosition(0) + _height);
            _line.SetPosition(1, _lineTest.GetPosition(0) + _dir * _straightLineLength + _height);
        }

        else if (_entitySport == IEntity.entitySport.basketball)
        {
            DrawPath(0, _basketSpawn);

            #region Old interesting code
            /*
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
            */
            #endregion
        }

        else if (_entitySport == IEntity.entitySport.baseball)
        {
            DrawPath(1, _baseballSpawn);
        }
    }


    /// <summary>
    /// 0 - idle, 1 - moving
    /// </summary>
    public void CameraFollowPlayer(int id)
    {
        if (_entityType == IEntity.entityType.player)
        {
            if (id == 0)
            {
                //_cameraPivot.position = _entityTransform.position;
                _cameraPivot.position = Vector3.Lerp(_cameraPivot.position, _camTargetIdle.position, _camIdleFollowSpeed * Time.deltaTime);
            }

            else if (id == 1)
            {
                _cameraPivot.position = Vector3.Lerp(_cameraPivot.position, _camTargetMove.position, _camMoveFollowSpeed * Time.deltaTime);
            }
        }
    }
    #endregion // player


    #region Shoot

    public void Shoot()
    {
        int _ballId = (int)_entitySport - 1;
        var _ball = Instantiate(_ballPrefabs[_ballId], _ballSpawns[_ballId].position, Quaternion.identity);
        var _ballRb = _ball.GetComponent<Rigidbody>();

        var _force = (InputManager.Instance._mousePickPoint - _ballSpawns[_ballId].position).normalized;

        if (_entitySport == IEntity.entitySport.basketball)
            Launch(_ballRb, _basketHeight);

        else if (_entitySport == IEntity.entitySport.football)
            _ballRb.AddForce(_force * _footballForce, ForceMode.Impulse);

        else if (_entitySport == IEntity.entitySport.baseball)
        {
            //_ballRb.AddForce(_force * _baseballForce, ForceMode.Impulse);
            Launch(_ballRb, _baseballHeight);
        }
    }

    IEnumerator ShootCooldown(float cooldown)
    {
        _canShoot = false;
        yield return new WaitForSeconds(cooldown);
        _canShoot = true;
    }


    #region Basketball Launch
    public void Launch(Rigidbody rb, float height)
    {
        rb.useGravity = false;
        Physics.gravity = Vector3.up * _gravity;
        rb.useGravity = true;
        rb.velocity = CalculateLaunchData(rb, height).initialVelocity;
    }


    LaunchData CalculateLaunchData(Rigidbody rb, float height)
    {
        
        float displacementY = _target.position.y - rb.position.y;
        Vector3 displacementXZ = new Vector3(_target.position.x - rb.position.x, 0, _target.position.z - rb.position.z);
        float time = Mathf.Sqrt(-2 * height / _gravity) + Mathf.Sqrt(2 * (displacementY - height) / _gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * _gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(_gravity), time);
    }


    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }


    /// <summary>
    /// 0-basket, 1-baseball
    /// </summary>
    public void DrawPath(int id, Rigidbody rb)
    {
        LaunchData launchData = CalculateLaunchData(rb, _basketHeight);

        if (id == 0)
        {
            var _dist = Vector3.Distance(_target.position, transform.position);
            _basketHeight = _dist * _heightMultiplier;
            _basketHeight = Mathf.Clamp(_basketHeight, 0, _maxBasketHeight);
            launchData = CalculateLaunchData(rb, _basketHeight);
        }
        
        else if (id == 1)
        {
            var _dist = Vector3.Distance(_target.position, transform.position);
            _baseballHeight = _dist * _heightMultiplier;
            _baseballHeight = Mathf.Clamp(_baseballHeight, 0, _maxBaseballHeight);
            launchData = CalculateLaunchData(rb, _baseballHeight);
        }        


        Vector3 previousDrawPoint = rb.position;
        _path = new Vector3[_resolution];

        for (int i = 0; i < _resolution; i++)
        {
            float simulationTime = i / (float)_resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * _gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = rb.position + displacement;
            _path[i] = drawPoint;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;

            if (_line.positionCount != _resolution)
                _line.positionCount = _resolution;

            if (i < _resolution)
                _line.SetPosition(i, drawPoint);
        }
    }
    #endregion // basket launch

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
            _path = new Vector3[_resolution];
            _line.positionCount = _resolution;
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

    void Gravity()
    {
        var _collisions = Physics.OverlapSphere(_groundDetector.position, _detectorRadius, _groundMask);

        if (_collisions != null && _collisions.Length > 0)
        {
            if (_isGrounded == false)
            {
                _internalMoveGravity = 1f;
                _isGrounded = true;
            }
        }

        else
        {
            if (_isGrounded == true)
            {
                _isGrounded = false;
            }
        }
    }


    public void TakeDamage(int ammount)
    {
        if (_health > 0)
        {
            _health -= ammount;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            //PlayAudio(0);

            if (_health <= 0)
            {
                // some end method
                _isDead = true;
            }
        }
    }


    public void PlayAudio()
    {
        //if (_source != null && _clip != null)
        //{
        //    _source.PlayOneShot(_clip, _volume);
        //}
    }


    private void OnDrawGizmos()
    {
        if (_groundDetector != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_groundDetector.position, _detectorRadius);
        }
    }

    #endregion


    #region AI

    public void SetAgentPath(Vector3 destination)
    {
        if (_agent.enabled == false)
        {
            _agent.enabled = true;
            _agent.isStopped = false;
            _agent.SetDestination(destination);
            _hasPath = true;
        }
    }


    public void StopAgent()
    {
        if (_agent.enabled)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            _agent.velocity = Vector3.zero;
            _agent.enabled = false;
            _hasPath = false;
        }
    }

    #endregion
}
