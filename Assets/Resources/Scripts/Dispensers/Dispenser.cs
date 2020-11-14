
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [Header("Parameters")]
    public IEntity.entitySport _sport;
    public GameObject _ballPrefab;
    public Transform _ballRestPos;
    public float _cooldown = 2f;

    [Header("Collisions")]
    public Transform _collisionTag;
    public float _radius;

    [Header("Animation")]
    [SerializeField] float _animDuration = 2f;
    [SerializeField] float _animMultiplier = 1f;
    [SerializeField] AnimationCurve _curve;
    public float _time;

    private void Update()
    {
        Animate();
        CheckCollisions();
    }

    public void CheckCollisions()
    {
        var _colliders = Physics.OverlapSphere(_collisionTag.position, _radius);

        if (_colliders != null && _colliders.Length > 0)
        {

        }
    }

    public void Animate()
    {
        if (_time < _animDuration)
        {
            var _pos = _ballRestPos.position;
            _pos.y = _curve.Evaluate(_time / _animDuration) * _animMultiplier;
            _ballRestPos.position = _pos;

            _time += Time.deltaTime;
        }

        else
            _time = 0;
    }

    private protected void OnDrawGizmos()
    {
        if (_collisionTag != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_collisionTag.position, _radius);
        }
    }
}
