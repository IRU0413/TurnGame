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

                // LayerType 선택 드롭다운 추가
                selectedLayerType = (LayerType)EditorGUILayout.EnumPopup("Select Layer Type", selectedLayerType);

                // Add Tilemap 버튼 추가
                if (GUILayout.Button("Add Tilemap"))
                {
                    roomData.AddTilemap(selectedLayerType);
                }
            }
        }
    }


}
