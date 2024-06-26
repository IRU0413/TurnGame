using Scripts.Generator;
using UnityEditor;
using UnityEngine;

namespace Scripts.Edit
{
    [CustomEditor(typeof(BaseSOGenerator), true)]
    public class BaseSOGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BaseSOGenerator generator = (BaseSOGenerator)target;
            if (GUILayout.Button("Create SO Data", GUILayout.Height(50)))
            {
                generator.CreateSOAssets();
            }
        }
    }


}
