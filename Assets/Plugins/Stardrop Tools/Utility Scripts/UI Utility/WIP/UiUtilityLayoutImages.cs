
using UnityEngine;

public class UiUtilityLayoutImages : MonoBehaviour
{
    [Header("Image Colors")]
    [SerializeField] UiUtilityLayoutColors _soColors;
    [Range(0, 1)][SerializeField] float _alpha = .2f;
    [SerializeField] string _imgName = "Layout Image";
    [SerializeField] Color[] _colors;
    [SerializeField] Transform[] _targetChildren;
    [SerializeField] UnityEngine.UI.Image[] _layoutImages;

    [Header("Create & Destroy")]
    bool _CreateImages;
    bool _DestroyImages;

    [Header("Enable & Disable")]
    bool _EnableImages;
    bool _DisableImages;

    [Header("Color Controller")]
    [SerializeField] bool _started = false;
    bool _GetColors;
    bool _SetAlpha;

    

    public void UtilityLayout()
    {
        if (_started == false)
            StartLayout();

        else
        {
            if (_GetColors == true)
                PopulateColorArray();

            if (_SetAlpha == true)
                SetColorAlpha();

            //----------------------------- Create & Destroy
            if (_CreateImages == true)
                CreateImages();

            if (_DestroyImages == true)
                DestroyImages();

            //----------------------------- Enable & Disable
            if (_EnableImages == true)
                EnableImages();

            if (_DisableImages == true)
                DisableImages();
        }
    }

    //============================================== Start & Populate Layout Arrays
    void StartLayout()
    {
        PopulateColorArray();
        SetColorAlpha();
        GetChildren();

        _started = true;
        Debug.Log("<color=orange> Layout Started </color>");
    }

    void PopulateColorArray()
    {
        _colors = _soColors.Colors;
        _GetColors = false;
    }

    public void SetColorAlpha()
    {
        if (_colors.Length > 0)
        {
            for (int i = 0; i < _colors.Length; i++)
            {
                var _tempCol = _colors[i];
                _tempCol.a = _alpha;
                _colors[i] = _tempCol;
            }

            if (_layoutImages != null)
            {
                for (int i = 0; i < _layoutImages.Length; i++)
                    if (_layoutImages[i] != null)
                    {
                        var _tempCol = _layoutImages[i].color;
                        _tempCol.a = _alpha;
                        _layoutImages[i].color = _tempCol;
                    }
            }
        }

        else
        {
            PopulateColorArray();
            SetColorAlpha();
        }

        _SetAlpha = false;
    }

    public void GetChildren()
    {
        _targetChildren = new Transform[transform.childCount];
        //_layoutImages = new UnityEngine.UI.Image[_targetChildren.Length];

        for (int i = 0; i < _targetChildren.Length; i++)
            _targetChildren[i] = transform.GetChild(i);
    }



    //============================================== Create, Destroy, Enable & Disable
    public void CreateImages()
    {
        if (_started == true)
        {
            DestroyImages();
            GetChildren();
            _layoutImages = new UnityEngine.UI.Image[_targetChildren.Length];

            for (int i = 0; i < _layoutImages.Length; i++)
            {
                var _name = _imgName + " - " + _targetChildren[i].name;

                var _go = new GameObject(_name, typeof(UnityEngine.UI.Image));
                _go.transform.SetParent(_targetChildren[i], false);
                _go.transform.SetAsFirstSibling();
                _go.AddComponent<UiUtilityImage>();

                var _copyTrans = _go.AddComponent<UiUtilityCopyTransform>();
                _copyTrans.m_Axis = UiUtilityBase.targetAxis.both;
                _copyTrans.GetParent();

                var _img = _go.GetComponent<UnityEngine.UI.Image>();
                _img.color = _colors[Random.Range(0, _colors.Length)];

                _layoutImages[i] = _img;
            }

            Debug.Log("<color=yellow> Layout Images: </color> <color=green> Created Images </color>");
        }

        else
            StartLayout();

        _CreateImages = false;
    }



    public void DestroyImages()
    {
        if (_started == true)
        {
            if (_layoutImages != null)
            {
                for (int i = 0; i < _layoutImages.Length; i++)
                    if (_layoutImages[i] != null)
                        DestroyImmediate(_layoutImages[i].gameObject);
            }

            Debug.Log("<color=yellow> Layout Images: </color> <color=orange> " + name + ": Destroyed Images </color>");
        }

        else
            StartLayout();

        _DestroyImages = false;
    }



    public void EnableImages()
    {
        if (_started == true)
        {
            if (_layoutImages.Length > 0)
                for (int i = 0; i < _layoutImages.Length; i++)
                    if (_layoutImages[i] != null)
                    {
                        _layoutImages[i].enabled = true;
                        _layoutImages[i].transform.SetAsFirstSibling();
                    }

            Debug.Log("<color=yellow> Layout Images: </color> <color=green> Enabled Images </color>");
        }

        else
            StartLayout();

        _EnableImages = false;
    }



    public void DisableImages()
    {
        if (_started == true)
        {
            if (_layoutImages.Length > 0)
                for (int i = 0; i < _layoutImages.Length; i++)
                {
                    if (_layoutImages[i] != null)
                        _layoutImages[i].enabled = false;
                }

            Debug.Log("<color=yellow> Layout Images: </color> <color=orange> Disabled Images </color>");
        }

        else
            StartLayout();

        _DisableImages = false;
    }

    /// <summary>
    /// 1 - create, 2 - destroy, 3 - enable, 4 - disable
    /// </summary>
    public void ManagerOverride(int id, bool value)
    {
        if (id == 0)
            _CreateImages = value;

        else if (id == 1)
            _DestroyImages = value;

        else if (id == 2)
            _EnableImages = value;

        else if (id == 3)
            _DisableImages = value;
    }

    public void OnDestroy()
    {
        DestroyImages();
    }
}
