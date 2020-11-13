
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UiUtilityCopyTransform : UiUtilityTransform
{
    [Range(0, 1)] [SerializeField] float _percentWidth = 1;
    [Range(0, 1)] [SerializeField] float _percentHeight = 1;    

    public override void GetValues()
    {
        base.GetValues();

        if (_clamp == true)
        {
            _targetWidth = Mathf.Clamp(_targetWidth, _minValues.x, _maxValues.x);
            _targetHeight = Mathf.Clamp(_targetHeight, _minValues.y, _maxValues.y);
        }

        _targetSize = new Vector2(_targetWidth, _targetHeight);
    }


    public override void UtilityTransform()
    {
        base.UtilityTransform();

        if (_container != null && _target != null)
        {
            GetValues();

            if (_axis == targetAxis.horizontal)
            {
                _target.sizeDelta = new Vector2(_targetWidth, _initSize.y);
            }

            else if (_axis == targetAxis.vertical)
            {
                _target.sizeDelta = new Vector2(_initSize.x, _targetHeight);
            }

            else if (_axis == targetAxis.both)
            {
                _target.sizeDelta = new Vector2(_targetWidth * _percentWidth, _targetHeight * _percentHeight);
            }
        }
    }
}


#if UNITY_EDITOR



#endif
