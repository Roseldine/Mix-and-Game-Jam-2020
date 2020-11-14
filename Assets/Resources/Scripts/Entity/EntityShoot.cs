
using UnityEngine;
using System.Collections;

public class EntityShoot : MonoBehaviour
{
    [Header("Entity")]
    public Entity _entity;
    [Tooltip("0 - basket, 1 - football, 2 - baseball")]
    public Transform[] _ballSpawnPoints;

    [Header("Ball Prefabs")]
    public GameObject[] _ballPrefabs;

    [Header("Projectile Prediction")]
    public AnimationCurve _animCurve;

    [Header("Projectile Prediction")]
    [SerializeField] ParabolaController _controller;
    public Transform[] _parabolaPoints;
    public float _distance = 70f;
    public float _duration = 2f;
    public float _minHeight;
    public float _maxHeight;
    public bool _hasBall;
    public bool _canShoot;

    private void Update()
    {
        UpdateParabolaPoints();
        Shoot();
    }

    void UpdateParabolaPoints()
    {
        // start point
        _parabolaPoints[0].position = _ballSpawnPoints[(int)_entity._entitySport + 1].position;

        // end point
        _parabolaPoints[2].position = InputManager.Instance._cameraRayHitPoint;

        // median point
        _parabolaPoints[1].position = ((_parabolaPoints[0].position + _parabolaPoints[2].position) / 2) + Vector3.up * _maxHeight;
    }

    void Shoot()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var _ball = Instantiate(_ballPrefabs[0], _ballSpawnPoints[(int)_entity._entitySport + 1].position, Quaternion.identity).transform;
            StartCoroutine(ShootParabola(_ball));
        }    
    }

    IEnumerator ShootParabola(Transform ball)
    {
        var _dist = Vector3.Distance(_parabolaPoints[0].position, _parabolaPoints[2].position);
        var _dur = _dist * _duration / _distance;
        var _start = _parabolaPoints[0].position;
        var _end = _parabolaPoints[2].position + Vector3.up * ball.localScale.y;
        Debug.Log(_dist + " in " + _dur);

        float t = 0;

        while (t < _dur)
        {
            ball.position = MathParabola.Parabola(_start, _end, _maxHeight, t / _dur);

            t += Time.deltaTime;
            yield return null;
        }
    }
}