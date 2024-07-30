using Scripts.Generator;
using UnityEditor;
using UnityEngine;

namespace Scripts.Edit
{
    [CustomEditor(typeof(ItemSOGenerator))]
    public class ItemSOGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ItemSOGenerator generator = (ItemSOGenerator)target;
            if (GUILayout.Button("Create Scriptable Object"))
            {
                generator.Create();
            }
        }
    }
}
