
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [Header("Parameters")]
    public GameObject _ballPrefab;
    public Transform _ballRestPos;
    public float _cooldown = 2f;

    [Header("Collisions")]
    public Transform _collisionTag;
    public float _radius;

    [Header("Animation")]
    [SerializeField] float _animDuration = 2f;
    [SerializeField] AnimationCurve _curve;

    private void Update()
    {
        
    }

    public void CheckCollisions()
    {
        var _colliders = Physics.OverlapSphere(_collisionTag.position, _radius);

        if (_colliders != null && _colliders.Length > 0)
        {

        }
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
