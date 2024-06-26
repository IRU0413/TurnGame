using Scripts.Generator;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitStatusSOGenerator))]
public class UnitStatusSOGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UnitStatusSOGenerator generator = (UnitStatusSOGenerator)target;

        base.OnInspectorGUI();
        if (GUILayout.Button("Create SO Data", GUILayout.Height(50)))
        {
            generator.CreateSOAsset();
        }
    }
}
