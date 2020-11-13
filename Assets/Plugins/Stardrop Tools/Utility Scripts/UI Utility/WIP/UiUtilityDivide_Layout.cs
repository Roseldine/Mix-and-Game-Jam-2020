
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UiUtilityDivide_Layout : UiUtilityBase
{
    public enum targetDistribution { custom, uniform }

    [SerializeField] RectTransform _target;

    [Header("Options")]
    [SerializeField] targetAxis _axis;
    [SerializeField] targetDistribution _distribuition;

    [Header("Children")]
    [SerializeField] UiDivideElement[] _elements;
    [SerializeField] List<RectTransform> _rects;

    [Header("Animation")]
    [HideInInspector]
    public int Id;
    [SerializeField] AnimationCurve _curve;

    public Vector2 _initValues;
    public Vector2 _maxValues;

    float _maxWidth;
    float _maxHeight;

    #region Parameters
    public UiDivideElement[] Elements { get { return _elements; } set { _elements = value; } }
    #endregion

    public void UtilityDivide()
    {
        GetTarget();
        UpdateElementPercents();
    }


    public void GetTarget()
    {
        if (_container == null)
            _container = GetComponent<RectTransform>();

        if (_axis == targetAxis.none || _axis == targetAxis.both)
            _axis = targetAxis.vertical;

        AddLayoutGroup();

        _maxValues = _container.sizeDelta;
        _maxWidth = _container.rect.width;
        _maxHeight = _container.rect.height;
    }

    public void AddLayoutGroup()
    {
        if (_axis == targetAxis.horizontal && GetComponent<UnityEngine.UI.HorizontalLayoutGroup>() == false)
        {
            DestroyImmediate(_container.GetComponent<UnityEngine.UI.LayoutGroup>());
            var _layout = _container.gameObject.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>();
            _layout.childControlHeight = true;
            _layout.childForceExpandWidth = true;
            _layout.childForceExpandHeight = false;
        }

        else if (_axis == targetAxis.vertical && GetComponent<UnityEngine.UI.VerticalLayoutGroup>() == false)
        {
            DestroyImmediate(_container.GetComponent<UnityEngine.UI.LayoutGroup>());
            var _layout = _container.gameObject.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
            _layout.childControlWidth = true;
            _layout.childForceExpandWidth = false;
            _layout.childForceExpandHeight = true;
        }
    }

    public void PopulateArray()
    {
        //----------- get all children
         _rects = new List<RectTransform>(_container.childCount); //List<RectTransform>

        foreach (Transform t in _container)
            _rects.Add(t.GetComponent<RectTransform>());

        //----------- remove utility images
        foreach (RectTransform _rect in _rects.ToList())
            if (_rect.GetComponent<UiUtilityImage>() == true)
                _rects.Remove(_rect);

        //----------- create divisable elements array
        var _children = _rects.ToArray();
        _elements = new UiDivideElement[_children.Length];

        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i] = new UiDivideElement();
            var _element = _elements[i];

            _element.Rect = _children[i];
            _element.Container = _container;
        }
    }

    void UpdateElementPercents()
    {
        foreach(UiDivideElement _element in _elements)
        {
            _element.Rect.sizeDelta = _maxValues * _element.Percent;
        }
    }

    public void Uniform()
    {
        if (_elements != null && _elements.Length > 1)
        {
            float _targetSize = 0;
            float _percent = 0;

            if (_axis == targetAxis.horizontal)
            { 
                _targetSize = _maxWidth / _elements.Length;
                _percent = _targetSize / _maxWidth;
            }

            else if (_axis == targetAxis.vertical)
            {
                _targetSize = _maxHeight / _elements.Length;
                _percent = _targetSize / _maxHeight;
            }

            foreach(UiDivideElement _element in _elements)
            {
                var _init = _element.Rect.rect.size;
                _element.Percent = _percent;

                if (_axis == targetAxis.horizontal)
                    _element.Rect.sizeDelta = new Vector2(_targetSize, _init.y);
                else if (_axis == targetAxis.vertical)
                    _element.Rect.sizeDelta = new Vector2(_init.x, _targetSize);
            }

            Debug.Log(_targetSize);
        }
    }


    public void AnimateUI(int id)
    {

    }
}

[System.Serializable]
public class UiDivideElement
{
    public RectTransform Rect;
    public RectTransform Container;
    [Range(0, 1)] public float Percent;
}


#if UNITY_EDITOR
[CustomEditor(typeof(UiUtilityDivide_Layout))]
public class CustomInspector_UiUtilityDivide_Layout : Editor
{
    int id;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UiUtilityDivide_Layout _target = (UiUtilityDivide_Layout)target;


        GUILayout.Space(5);
        GUILayout.Label("Layout", EditorStyles.boldLabel);

        //---------------------------------- Preparation
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Get Container"))
            _target.GetTarget();
        if (GUILayout.Button("Populate Arrays"))
            _target.PopulateArray();
        if (GUILayout.Button("Divide Uniformly"))
            _target.Uniform();

        GUILayout.EndHorizontal();

        //---------------------------------- Animation
        GUILayout.Space(5);
        GUILayout.Label("Animation", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();

        //GUILayout.Label("Animation ID", EditorStyles.boldLabel);
        if (GUILayout.Button("Animate ID"))
            _target.Uniform();

        id = EditorGUILayout.IntSlider(id, 0, _target.Elements.Length - 1, GUILayout.Width(200f));        

        GUILayout.EndHorizontal();

        if (GUI.changed)
            EditorUtility.SetDirty(_target);
    }
}
#endif
