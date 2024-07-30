//c# Example (LookAtPointEditor.cs)
using UnityEditor;

namespace Scripts.Edit
{
    [CustomEditor(typeof(LookAtPoint))]
    [CanEditMultipleObjects]
    public class LookAtPointEditor : Editor
    {
        SerializedProperty lookAtPoint;

        void OnEnable()
        {
            lookAtPoint = serializedObject.FindProperty("lookAtPoint");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(lookAtPoint);
            serializedObject.ApplyModifiedProperties();
        }
    }
}