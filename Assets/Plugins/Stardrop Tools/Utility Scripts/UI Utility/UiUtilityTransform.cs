
using UnityEngine;


public class UiUtilityTransform : UiUtilityBase
{
    [SerializeField] protected RectTransform _target;
    [SerializeField] protected targetAxis _axis;

    [Range(0, 1)] [SerializeField] protected float _percent = 1;
    [SerializeField] protected bool _clamp;

    [Header("Display Values")]
    [SerializeField] protected Vector2 _minValues;
    [SerializeField] protected Vector2 _maxValues;
    [SerializeField] protected Vector2 _initSize;

    [SerializeField] protected float _targetWidth;
    [SerializeField] protected float _targetHeight;
    [SerializeField] protected Vector2 _targetSize;

    bool _getParent;
    bool _getChild;


    #region Parameters
    public targetAxis m_Axis { get { return _axis; } set { _axis = value; } }
    public bool m_GetParent { get { return _getParent; } set { _getParent = value; } }
    public bool m_GetChild { get { return _getChild; } set { _getChild = value; } }
    #endregion


    public virtual void UtilityTransform()
    {
        
    }

    public virtual void GetValues()
    {
        _initSize = _target.sizeDelta;
        _maxValues = _container.rect.size;

        _targetWidth = _percent * _maxValues.x;
        _targetHeight = _percent * _maxValues.y;
    }


    public void GetParent()
    {
        _container = transform.parent.GetComponent<RectTransform>();
        _target = GetComponent<RectTransform>();

        if (_axis == targetAxis.none)
            _axis = targetAxis.both;

        Debug.Log("<color=yellow> Utility Transform: </color> <color=orange> " + name + " Got Parent </color>");

        _getParent = false;
    }

    public void GetChild()
    {
        _container = GetComponent<RectTransform>();
        foreach (Transform t in _container)
            if (t.GetComponent<RectTransform>() == true)
            {
                _target = t.GetComponent<RectTransform>();
                break;
            }

        Debug.Log("<color=yellow> Utility Transform: </color> <color=blue> " + name + " Got Child </color>");

        _getChild = false;
    }

    //==================================================================== Important Checker
    /*
     
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
                _target.sizeDelta = new Vector2(_targetWidth, _targetHeight);
            }

     */
}