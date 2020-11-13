
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UiUtilityDivide : UiUtilityBase
{
    public enum targetAnchor { topLeft, topCenter, topRight, midLeft, midCenter, midRight, botLeft, botCenter, botRight }
    public enum dividePreset { none, div2, div3 }
    public enum targetDistribution { uniform, custom }

    [SerializeField] RectTransform _target;
    [SerializeField] targetAxis _axis;
    [SerializeField] targetAnchor _anchor;
    [SerializeField] targetDistribution _distribution;
    bool _getChildren;

    [SerializeField] float _minSize;
    [SerializeField] Vector2 _maxSize;
    [SerializeField] Vector2 _targetAnchor;
    [Range(0, 1)] [SerializeField] float _percent;
    [Range(0, 1)] [SerializeField] float[] _percentElements;
    [SerializeField] RectTransform[] _targetElements;
    [SerializeField] float[] _remainingPercents;
    [SerializeField] float[] _remainingSpaces;

    float _maxWidth;
    float _maxHeight;
    int _totalElements;



    #region Parameters

    public float[] PercentElements { get { return _percentElements; } set { _percentElements = value; } }

    #endregion



    public void UtilityDivide()
    {
        GetChildren();

        if (_container != null && _totalElements > 1)
        {
            if (_target == null)
            {
                _maxSize = _container.rect.size;
                _maxWidth = Mathf.Clamp(_maxSize.x, _minSize, UiUtilityManager.Instance.CanvasRect.rect.width);
                _maxHeight = Mathf.Clamp(_maxSize.y, _minSize, UiUtilityManager.Instance.CanvasRect.rect.height);
                _container.sizeDelta = new Vector2(_maxWidth, _maxHeight);
            }

            else
            {
                _maxSize = _target.rect.size;
                _maxWidth = Mathf.Clamp(_maxSize.x, _minSize, UiUtilityManager.Instance.CanvasRect.rect.width);
                _maxHeight = Mathf.Clamp(_maxSize.y, _minSize, UiUtilityManager.Instance.CanvasRect.rect.height);
                _target.sizeDelta = new Vector2(_maxWidth, _maxHeight);
            }
            

            ChangeAnchor();
            Divide();
        }
    }



    protected void Divide()
    {
        if (_totalElements == 2 && _percentElements != null)
        {
            _percentElements = null;
            _remainingPercents = null;
            _remainingSpaces = null;
        }

        else if (_totalElements >= 3)
        {
            _percent = _maxHeight / _totalElements / _maxHeight;

            if (_percentElements == null)
                CreateArrays();

            else if (_percentElements.Length != _totalElements)
                CreateArrays();
        }

        for (int i = 0; i < _totalElements; i++)
        {
            // 1st - Get element
            var _element = _targetElements[i];
            SetAnchor(_element);

            // 2nd - Set dimensions
            SetDimensions(_element, i);

            // 3rd - Set positions
            SetPosition(_element, i);
        }
    }

    void CreateArrays()
    {
        // get previously created arrays to merge with new ones
        var _tempPercentElements = _percentElements;
        var _tempRemPercents = _percentElements;
        var _tempRemSpaces = _percentElements;

        // create new ones
        _percentElements = new float[_totalElements];
        _remainingPercents = new float[_totalElements];
        _remainingSpaces = new float[_totalElements];

        // merge previous data
        if (_totalElements < _tempPercentElements.Length)
            for (int i = 0; i < _totalElements; i++)
            {
                _percentElements[i] = _tempPercentElements[i];
                _remainingPercents[i] = _tempRemPercents[i];
                _remainingSpaces[i] = _tempRemSpaces[i];
            }
        else
            for (int i = 0; i < _tempPercentElements.Length; i++)
            {
                _percentElements[i] = _tempPercentElements[i];
                _remainingPercents[i] = _tempRemPercents[i];
                _remainingSpaces[i] = _tempRemSpaces[i];
            }
    }



    protected void SetDimensions(RectTransform _element, int _index)
    {
        // 1st element must always be at Local Zero
        if (_index == 0)
            _element.anchoredPosition = Vector2.zero;

        //========================================================= Divide by 2
        if (_totalElements == 2 && _index == 0)
        {
            if (_distribution == targetDistribution.uniform && _percent != .5f)
                _percent = .5f;

            if (_axis == targetAxis.vertical)
            {
                _element.sizeDelta = new Vector2(_maxWidth, _maxHeight * _percent);
                _targetElements[_index + 1].sizeDelta = new Vector2(_maxWidth, _maxHeight - _element.sizeDelta.y);
            }

            else if (_axis == targetAxis.horizontal)
            {
                _element.sizeDelta = new Vector2(_maxWidth * _percent, _maxHeight);
                _targetElements[_index + 1].sizeDelta = new Vector2(_maxWidth - _element.sizeDelta.x, _maxHeight);
            }
        }


        //========================================================= Divide by 3+
        else if (_totalElements >= 3)
        {
            //----------------------------------------------------- Division Uniform
            if (_distribution == targetDistribution.uniform)
            {
                var _targetHeight = _maxHeight / _totalElements;
                var _targetWidth = _maxWidth / _totalElements;

                if (_axis == targetAxis.vertical)
                {
                    _element.sizeDelta = new Vector2(_maxWidth, _targetHeight);
                    _percent = _targetHeight / _maxHeight;
                }

                else if (_axis == targetAxis.horizontal)
                {
                    _element.sizeDelta = new Vector2(_targetWidth, _maxHeight);
                    _percent = _targetWidth / _maxWidth;
                }

                _percentElements[_index] = _percent;
            }

            //----------------------------------------------------- Division Custom
            else if (_distribution == targetDistribution.custom)
            {
                if (_axis == targetAxis.vertical)
                {
                    if (_index == 0)
                    {
                        _element.sizeDelta = new Vector2(_maxWidth, _maxHeight * _percentElements[_index]);
                        _remainingPercents[_index] = 1 - _percentElements[_index];
                        _remainingSpaces[_index] = _maxHeight - _element.sizeDelta.y;
                    }

                    else
                    {
                        _element.sizeDelta = new Vector2(_maxWidth, _remainingSpaces[_index - 1] * _percentElements[_index]);
                        _remainingPercents[_index] = 1 - ((_percentElements[_index - 1] + _percentElements[_index]) / 2);
                        _remainingSpaces[_index] = _remainingSpaces[_index - 1] - _element.sizeDelta.y;
                    }
                }

                else if (_axis == targetAxis.horizontal)
                {
                    if (_index == 0)
                    {
                        _element.sizeDelta = new Vector2(_maxWidth * _percentElements[_index], _maxHeight);
                        _remainingPercents[_index] = 1 - _percentElements[_index];
                        _remainingSpaces[_index] = _maxWidth - _element.sizeDelta.x;
                    }

                    else
                    {
                        _element.sizeDelta = new Vector2(_remainingSpaces[_index - 1] * _percentElements[_index], _maxHeight);
                        _remainingPercents[_index] = 1 - ((_percentElements[_index - 1] + _percentElements[_index]) / 2);
                        _remainingSpaces[_index] = _remainingSpaces[_index - 1] - _element.sizeDelta.x;
                    }
                }
            }
        }
    }



    protected void SetPosition(RectTransform _targetRect, int _index)
    {
        if (_index == 0)
            _targetRect.anchoredPosition = Vector2.zero;

        else
        {
            var _prevElement = _targetElements[_index - 1];
            var _prevPos = _prevElement.anchoredPosition;
            var _targetPos = Vector2.zero;

            if (_axis == targetAxis.horizontal)
                _targetPos = new Vector2(_prevPos.x + _prevElement.rect.width, _prevPos.y);
            else if (_axis == targetAxis.vertical)
                _targetPos = new Vector2(_prevPos.x, _prevPos.y - _prevElement.rect.height);

            _targetRect.anchoredPosition = _targetPos;
        }
    }



    public void GetChildren()
    {
        if (_axis == targetAxis.none)
            _axis = targetAxis.vertical;

        if (_container == null)
            _container = GetComponent<RectTransform>();

        int _total = 0;

        for (int i = 0; i < _container.childCount; i++)
            if (_container.GetChild(i).GetComponent<UiUtilityImage>() == false)
                _total++;

        _totalElements = _total;

        if (_targetElements == null)
            _getChildren = true;

        _targetElements = new RectTransform[_totalElements];

        for (int i = 0; i < _totalElements; i++)
        {
            _targetElements[i] = _container.GetChild(i).GetComponent<RectTransform>();
        }
    }


    //========================================================= -- Anchor Change
    protected void SetAnchor(RectTransform _targetRect)
    {
        _targetRect.anchorMin = _targetAnchor;
        _targetRect.anchorMax = _targetAnchor;
        _targetRect.pivot = _targetAnchor;
    }

    protected void ChangeAnchor()
    {
        switch (_anchor)
        {
            //----------------------------------------------------- top
            case targetAnchor.topLeft:
                _targetAnchor = new Vector2(0, 1);
                break;

            case targetAnchor.topRight:
                _targetAnchor = new Vector2(1, 1);
                break;

            case targetAnchor.topCenter:
                _targetAnchor = new Vector2(.5f, 1);
                break;

            //----------------------------------------------------- mid
            case targetAnchor.midLeft:
                _targetAnchor = new Vector2(0, .5f);
                break;

            case targetAnchor.midRight:
                _targetAnchor = new Vector2(1, .5f);
                break;

            case targetAnchor.midCenter:
                _targetAnchor = new Vector2(.5f, .5f);
                break;

            //----------------------------------------------------- bot
            case targetAnchor.botLeft:
                _targetAnchor = new Vector2(0, 0);
                break;

            case targetAnchor.botRight:
                _targetAnchor = new Vector2(1, 0);
                break;

            case targetAnchor.botCenter:
                _targetAnchor = new Vector2(.5f, 0);
                break;
        }
    }

    /*
    static void DrawStringGUI(string text, Vector3 worldPos, Color? color = null)
    {
        UnityEditor.Handles.BeginGUI();
        if (color.HasValue) GUI.color = color.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
        UnityEditor.Handles.EndGUI();
    }
    */
}


#if UNITY_EDITOR
[CustomEditor(typeof(UiUtilityDivide_Layout))]
public class CustomInspector_UiUtilityDivide : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UiUtilityDivide_Layout _target = (UiUtilityDivide_Layout)target;

        if (GUI.changed)
            EditorUtility.SetDirty(_target);
    }
}
#endif