using Scripts.Generator;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillSOGenerator))]
public class SkillSOGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        SkillSOGenerator generator = (SkillSOGenerator)target;

        base.OnInspectorGUI();
        if (GUILayout.Button("Create SO Data", GUILayout.Height(50)))
        {
            generator.CreateSOAsset();
        }
    }
}
