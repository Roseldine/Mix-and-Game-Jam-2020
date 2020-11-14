
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Type")]
    public int _entityId;
    public IEntity.entityType _entityType;
    public IEntity.entitySport _entitySport;

    [Header("Entity Movement")]
    public float _moveSpeed = 5;
    public float _lookSpeed = 10;
    public float _jumpForce = 20f;
    public float _shootCooldown = 1f;

    [Header("Gravity")]
    public LayerMask _detectionMask;
    public float _groudDetectorRadius = .1f;
    public float _gravitySmoothTime = .75f;
    public float _gravityForce = 100f;
    public AnimationCurve _gravityCurve;
    public bool _isGrounded;
    float _time;

    [Header("Components")]
    public Rigidbody _rigidbody;
    public BallLauncher _ballLauncher;
    public Transform _groundDetector;
    public Transform _graphicContainer;

    [Header("Graphic Animator")]
    public GameObject[] _sportGear;
    public Transform _graphic;
    public Animator _animator;
    public string _graphicTag = "Graphic";

    [Header("Animator States")]
    public float _crossFadeDuration = .25f;
    [Tooltip("0-idle, 1-run, 2-jump start, 3-falling, 4-death, 5-basket, 6-footbal, 7-baseball")]
    public string[] _animatorStates;
    [Tooltip("0-idle, 1-run, 2-jump start, 3-falling, 4-death, 5-basket, 6-footbal, 7-baseball")]
    public string[] _animatorBools;
    [Tooltip("0-basket, 1-footbal, 2-baseball")]
    public float[] _shootAnimDuration;
    public bool _isShooting;

    [Header("Camera Dependencies")]
    public Transform _cameraPivotX;
    public Transform _cameraPivotY;

    private void Update()
    {
        UpdateEntity();
    }


    public virtual void UpdateEntity()
    {
        Gravity();
    }


    /// <summary>
    /// 0-none, 1-basket, 2-football, 3-baseball
    /// </summary>
    public void SetSport(int id)
    {
        _entitySport = (IEntity.entitySport)id;

        for (int i = 0; i < _sportGear.Length; i++)
            _sportGear[i].SetActive(false);

        if (id > 0)
            _sportGear[id].SetActive(false);
    }


    #region Utility
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

    public void Gravity()
    {
        var _collisions = Physics.OverlapSphere(_groundDetector.position, _groudDetectorRadius, _detectionMask);

        if (_collisions != null && _collisions.Length > 0)
        {
            if (_isGrounded == false)
            {
                _time = 0;
                _isGrounded = true;
            }
        }

        else
        {
            if (_isGrounded == true)
                _isGrounded = false;

            if (_time < _gravitySmoothTime)
            {
                _time += Time.deltaTime;
            }
            
            _rigidbody.AddForce(Vector3.down * _gravityForce * _gravityCurve.Evaluate(_time / _gravitySmoothTime));
        }
    }



    private protected void OnDrawGizmos()
    {
        if (_groundDetector != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundDetector.position, _groudDetectorRadius);
        }
    }
    #endregion


    #region Animation

    /// <summary>
    /// 0-idle, 1-run, 2-jump start, 3-falling, 4-death, 5-basket, 6-footbal, 7-baseball
    /// </summary>
    public void PlayAnimation(int id)
    {
        if (_animator.GetBool(_animatorBools[id]) == false)
        {
            for (int i = 0; i < _animatorBools.Length; i++)
                _animator.SetBool(_animatorBools[i], false);

            _animator.SetBool(_animatorBools[id], true);
            _animator.CrossFade(_animatorStates[id], _crossFadeDuration);
        }
    }


    /// <summary>
    /// 5-basket, 6-footbal, 7-baseball
    /// </summary>
    public void PlayShootAnimation(int id)
    {
        PlayAnimation(id + 5);
        StartCoroutine(ShootCooldown());
    }

    protected IEnumerator ShootCooldown()
    {
        _isShooting = true;

        yield return new WaitForSeconds(_shootCooldown);
        _isShooting = false;
    }

    protected IEnumerator ShootCooldown(float cooldown)
    {
        _isShooting = true;

        yield return new WaitForSeconds(cooldown);
        _isShooting = false;
    }



    #endregion
}