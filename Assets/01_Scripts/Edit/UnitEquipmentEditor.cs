using Scripts.Cores.Item.Gear;
using Scripts.Cores.Unit.Assemblies;
using Scripts.Enums;
using UnityEditor;
using UnityEngine;

namespace Scripts.Edit
{
    [CustomEditor(typeof(UnitEquipment))]
    public class UnitEquipmentEditor : Editor
    {
        private GearItemCore selectedGear;
        private GearType selectedGearType;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UnitEquipment equipment = (UnitEquipment)target;

            GUILayout.Space(10);
            GUILayout.Label("= 장비 관리 =", EditorStyles.boldLabel);

            selectedGear = (GearItemCore)EditorGUILayout.ObjectField("장비 아이템", selectedGear, typeof(GearItemCore), true);

            if (selectedGear != null)
            {
                if (GUILayout.Button("장착"))
                {
                    equipment.Equip(selectedGear);
                    selectedGear = null; // 장착 후 선택 해제
                }
            }

            GUILayout.Space(10);
            GUILayout.Label("= 장비 해제 =", EditorStyles.boldLabel);

            selectedGearType = (GearType)EditorGUILayout.EnumPopup("장비 타입", selectedGearType);

            if (GUILayout.Button("해제"))
            {
                equipment.Unequip(selectedGearType);
            }
        }
    }
}