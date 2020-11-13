
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum entityType { none, player, enemy }

    [Header("Entity Type")]
    public int _entityId;
    public entityType _entityType;

    [Header("Entity Movement")]
    public float _moveSpeed = 5;
    public float _lookSpeed = 10;
    public float _jumpForce = 20f;

    [Header("Gravity")]
    public LayerMask _detectionMask;
    public float _groudDetectorRadius = .1f;
    public float _gravitySmoothTime = .75f;
    public float _gravityForce = 100f;
    //public AnimationCurve _jumpCurve;
    public AnimationCurve _gravityCurve;
    public bool _isGrounded;
    float _time;

    [Header("Components")]
    public Rigidbody _rigidbody;
    public Transform _groundDetector;

    private void Update()
    {
        UpdateEntity();
    }


    public virtual void UpdateEntity()
    {
        Gravity();
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
}