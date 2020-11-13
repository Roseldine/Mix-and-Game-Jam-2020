
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UiUtilityManager))]
public class CustomInspector_UiUtility : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UiUtilityManager _target = (UiUtilityManager)target;

        //----------------------------------------- Create & Destroy
        GUILayout.Label("Create & Destroy");

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Images"))
            _target.CreateImages();

        if (GUILayout.Button("Destroy Images"))
            _target.DestroyImages();
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Destroy ALL Images - Useful to destroy forgotten Images"))
            _target.DestroyImages();

        //----------------------------------------- Enable & Disable
        GUILayout.Label("Enable & Disable");

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Enable Images"))
            _target.EnableImages();

        if (GUILayout.Button("Disable Images"))
            _target.DisableImages();
        GUILayout.EndHorizontal();

        if (GUI.changed)
            EditorUtility.SetDirty(_target);
    }
}

//===================================================================== Transform
#region Transform

// transform base
[CustomEditor(typeof(UiUtilityTransform))]
public class CustomInspect_UiUtilityTransform : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UiUtilityTransform _target = (UiUtilityTransform)target;

        if (GUILayout.Button("Get Parent"))
            _target.GetParent();

        if (GUILayout.Button("Get Child"))
            _target.GetChild();

        if (GUI.changed)
            EditorUtility.SetDirty(_target);
    }
}

// Copy transform
[CustomEditor(typeof(UiUtilityCopyTransform))]
public class CustomInspect_UiUtilityCopyTransform : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UiUtilityTransform _target = (UiUtilityCopyTransform)target;

        if (GUILayout.Button("Get Parent"))
            _target.GetParent();

        if (GUILayout.Button("Get Child"))
            _target.GetChild();

        if (GUI.changed)
            EditorUtility.SetDirty(_target);
    }
}

// Fit transform
[CustomEditor(typeof(UiUtilityFitTransform))]
public class CustomInspect_UiUtilityFitTransform : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UiUtilityFitTransform _target = (UiUtilityFitTransform)target;

        if (GUILayout.Button("Get Parent"))
        {
            _target.GetParent();
            _target.m_Axis = UiUtilityBase.targetAxis.both;
        }

        if (GUILayout.Button("Get Child"))
        {
            _target.GetChild();
            _target.m_Axis = UiUtilityBase.targetAxis.both;
        }

        if (GUI.changed)
            EditorUtility.SetDirty(_target);
    }
}

#endregion



//===================================================================== Divide
#region Divide

[CustomEditor(typeof(UiUtilityDivide))]
public class CustomInspect_UiUtilityDivide : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UiUtilityDivide _target = (UiUtilityDivide)target;

        if (GUILayout.Button("Get Children"))
            _target.GetChildren();

        if (GUI.changed)
            EditorUtility.SetDirty(_target);
    }
}

#endregion



//===================================================================== Image Layout
#region Image LayoutElement

[CustomEditor(typeof(UiUtilityLayoutImages))]
public class CustomInspect_UiUtilityImageLayout : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UiUtilityLayoutImages _target = (UiUtilityLayoutImages)target;

        if (GUILayout.Button("Get Children"))
            _target.GetChildren();

        if (GUILayout.Button("Set Alpha"))
            _target.SetColorAlpha();

        //----------------------------------------- Create & Destroy
        GUILayout.Label("Create & Destroy");

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Images"))
            _target.CreateImages();

        if (GUILayout.Button("Destroy Images"))
            _target.DestroyImages();
        GUILayout.EndHorizontal();

        //----------------------------------------- Enable & Disable
        GUILayout.Label("Enable & Disable");

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Enable Images"))
            _target.EnableImages();

        if (GUILayout.Button("Disable Images"))
            _target.DisableImages();
        GUILayout.EndHorizontal();

        if (GUI.changed)
            EditorUtility.SetDirty(_target);
    }
}

#endregion