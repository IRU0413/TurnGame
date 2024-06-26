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
            GUILayout.Label("= ��� ���� =", EditorStyles.boldLabel);

            selectedGear = (GearItemCore)EditorGUILayout.ObjectField("��� ������", selectedGear, typeof(GearItemCore), true);

            if (selectedGear != null)
            {
                if (GUILayout.Button("����"))
                {
                    equipment.Equip(selectedGear);
                    selectedGear = null; // ���� �� ���� ����
                }
            }

            GUILayout.Space(10);
            GUILayout.Label("= ��� ���� =", EditorStyles.boldLabel);

            selectedGearType = (GearType)EditorGUILayout.EnumPopup("��� Ÿ��", selectedGearType);

            if (GUILayout.Button("����"))
            {
                equipment.Unequip(selectedGearType);
            }
        }
    }
}