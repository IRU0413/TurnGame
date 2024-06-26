using Scripts.Manager;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillManager))]
public class SkillSpawnerEditor : Editor
{
    int spawnID = -1;
    public override void OnInspectorGUI()
    {
        SkillManager generator = (SkillManager)target;

        base.OnInspectorGUI();
        if (generator.IsInitialized)
        {
            spawnID = EditorGUILayout.IntField("Spawn Skill ID", spawnID);
            if (GUILayout.Button("Spawn Skill", GUILayout.Height(50)))
            {
                generator.Spawn(spawnID);
            }
        }
    }
}
