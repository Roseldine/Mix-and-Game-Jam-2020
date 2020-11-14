
using System.Collections;
using UnityEngine;

public class EntityAnimation : MonoBehaviour
{
    [Header("Animatior")]
    public Animator _animator;
    [Range(0, 1)] public float _crossfade = .25f;

    [Header("Animatior")]

    [Tooltip("0-idle, 1-run, 2-falling, 4-basket, 5-footbal, 6-baseball, 7-death")]
    public string[] _animStates;
    [Tooltip("0-idle, 1-run, 2-falling, 4-basket, 5-footbal, 6-baseball, 7-death")]
    public string[] _animBools;
    [Tooltip("0-movement, 1-shoot")]
    public string[] _animTags;


    /// <summary>
    /// 0-idle, 1-run, 2-falling, 4-basket, 5-footbal, 6-baseball, 7-death
    /// </summary>
    public void PlayAnimation(int id)
    {
        if (_animator != null)
        {
            if (_animator.GetBool(_animBools[id]) == false)
            {
                ResetAllAnimations();
                _animator.SetBool(_animBools[id], true);
                _animator.CrossFade(_animStates[id], _crossfade);
            }
        }
    }


    void ResetAllAnimations()
    {
        for (int i = 0; i < _animBools.Length; i++)
            _animator.SetBool(_animBools[i], false);
    }
}