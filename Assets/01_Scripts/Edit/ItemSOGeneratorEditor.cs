using Scripts.Generator;
using UnityEditor;
using UnityEngine;

namespace Scripts.Edit
{
    [CustomEditor(typeof(SOGenerator), true)]
    public class ItemSOGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var generator = (SOGenerator)target;
            if (GUILayout.Button("Create Scriptable Object"))
            {
                generator.CreateEditBtn();
            }
        }
    }
}
