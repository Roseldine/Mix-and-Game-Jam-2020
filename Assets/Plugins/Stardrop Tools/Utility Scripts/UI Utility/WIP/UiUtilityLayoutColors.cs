
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Layout Colors", menuName = "UI Utility / Create Layout Colors")]
public class UiUtilityLayoutColors : ScriptableObject
{
    public int colorNumber;
    public Color[] Colors;

    public void PopulateColors()
    {
        Colors = new Color[colorNumber];

        for (int i = 0; i < colorNumber; i++)
            Colors[i] = Color.white * 1;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UiUtilityLayoutColors))]
public class CE_UiUtilityLayoutColors : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UiUtilityLayoutColors _target = (UiUtilityLayoutColors)target;

        GUILayout.Space(50);
        if (GUILayout.Button("Populate Colors"))
            _target.PopulateColors();
    }
}
#endif