using Scripts.Data;
using Scripts.Enums;
using UnityEditor;
using UnityEngine;

namespace Scripts.Edit
{
    [CustomEditor(typeof(RoomData))]
    public class RoomDataEditor : Editor
    {
        private LayerType selectedLayerType = LayerType.Background;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!EditorApplication.isPlaying)
            {
                RoomData roomData = (RoomData)target;

                // LayerType ���� ��Ӵٿ� �߰�
                selectedLayerType = (LayerType)EditorGUILayout.EnumPopup("Select Layer Type", selectedLayerType);

                // Add Tilemap ��ư �߰�
                if (GUILayout.Button("Add Tilemap"))
                {
                    roomData.AddTilemap(selectedLayerType);
                }
            }
        }
    }


}
