
using UnityEngine;

[ExecuteInEditMode]
public class UiUtilityManager : MonoBehaviour
{
    public static UiUtilityManager Instance;
    public RectTransform CanvasRect;

    [Header("Should this Execute in Edit Mode?")]
    public bool _runInEditMode;

    [Header("UI Utility Objects")]
    public int _utilityObjectCount;
    public UiUtilityTransform[] _utilityTransforms;
    public UiUtilityDivide[] _utilityDivides;
    public UiUtilityDivide_Layout[] _utilityDivides_Layout;
    public UiUtilityLayoutImages[] _utilityLayoutImageControllers;
    public UiUtilityImage[] _utilityLayoutImages;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (Instance == null)
            Instance = this;

        if (_runInEditMode)
        {
            GetUiElements();
            ApplyUtilities();
        }
    }


    void GetUiElements()
    {
        _utilityTransforms = FindObjectsOfType<UiUtilityTransform>();

        _utilityDivides = FindObjectsOfType<UiUtilityDivide>();
        _utilityDivides_Layout = FindObjectsOfType<UiUtilityDivide_Layout>();

        _utilityLayoutImageControllers = FindObjectsOfType<UiUtilityLayoutImages>();
        _utilityLayoutImages = FindObjectsOfType<UiUtilityImage>();

        _utilityObjectCount = _utilityTransforms.Length + _utilityDivides_Layout.Length;
    }


    void ApplyUtilities()
    {
        foreach (UiUtilityTransform _ut in _utilityTransforms)
            _ut.UtilityTransform();

        foreach (UiUtilityDivide _ud in _utilityDivides)
            _ud.UtilityDivide();

        foreach (UiUtilityDivide_Layout _ud in _utilityDivides_Layout)
            _ud.UtilityDivide();

        LayoutManager();
    }


    public void ForceRefresh()
    {

    }


    void LayoutManager()
    {
        foreach (UiUtilityLayoutImages _uli in _utilityLayoutImageControllers)
        {
            _uli.UtilityLayout();
        }
    }

    public void CreateImages()
    {
        if (_utilityLayoutImageControllers != null)
            for (int i = 0; i < _utilityLayoutImageControllers.Length; i++)
                _utilityLayoutImageControllers[i].CreateImages();
    }

    public void DestroyImages()
    {
        if (_utilityLayoutImageControllers != null)
            for (int i = 0; i < _utilityLayoutImageControllers.Length; i++)
                _utilityLayoutImageControllers[i].DestroyImages();

        _utilityLayoutImages = FindObjectsOfType<UiUtilityImage>();

        if (_utilityLayoutImages != null)
            for (int i = 0; i < _utilityLayoutImages.Length; i++)
                DestroyImmediate(_utilityLayoutImages[i].gameObject);
    }

    public void EnableImages()
    {
        if (_utilityLayoutImageControllers != null)
            for (int i = 0; i < _utilityLayoutImageControllers.Length; i++)
                _utilityLayoutImageControllers[i].EnableImages();
    }

    public void DisableImages()
    {
        if (_utilityLayoutImageControllers != null)
            for (int i = 0; i < _utilityLayoutImageControllers.Length; i++)
                _utilityLayoutImageControllers[i].DisableImages();
    }
}
