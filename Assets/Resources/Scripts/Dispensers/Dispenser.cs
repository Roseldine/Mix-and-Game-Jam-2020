
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [Header("Parameters")]
    public GameObject _ballPrefab;
    public Transform _ballRestPos;
    public float _cooldown;

    [Header("Animation")]
    [SerializeField] float _animDuration;
    [SerializeField] AnimationCurve _curve;


}
