
using UnityEngine;

public class UiUtilityFitTransform : UiUtilityTransform
{
    public override void GetValues()
    {
        base.GetValues();

        if (_axis != targetAxis.both)
            _axis = targetAxis.both;

        if (_clamp == false)
            _clamp = true;

        if (_clamp == true)
        {
            float _max = 0;
            if (_maxValues.x < _maxValues.y)
                _max = _maxValues.x;
            else
                _max = _maxValues.y;

            _targetWidth = Mathf.Clamp(_targetWidth, _minValues.x, _max);
            _targetHeight = Mathf.Clamp(_targetHeight, _minValues.y, _max);
        }

        _targetSize = new Vector2(_targetWidth, _targetHeight);
    }


    public override void UtilityTransform()
    {
        base.UtilityTransform();

        if (_container != null && _target != null)
        {
            GetValues();

            float _avg = (_targetWidth + _targetHeight) / 2;
            _target.sizeDelta = new Vector2(_avg, _avg);
        }
    }
}
