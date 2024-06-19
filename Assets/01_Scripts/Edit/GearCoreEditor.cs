using Scripts.Cores.Item.Gear;
using Scripts.Cores.Unit;
using Scripts.Enums;
using Scripts.Pattern;
using UnityEditor;
using UnityEngine;

namespace Scripts.Edit
{
    [CustomEditor(typeof(GearItemCore))]
    [CanEditMultipleObjects]
    public class GearCoreEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GearItemCore gearCore = (GearItemCore)target;

            if (gearCore._status == null)
            {
                gearCore._status = new SerializableDictionary<UnitAbilityType, GearAbility>();
            }

            if (GUILayout.Button("Add Entry"))
            {
                int start = (int)UnitAbilityType.Health;
                int end = (int)UnitAbilityType.ActionSpeed;
                for (int i = start; i <= end; i++)
                {
                    UnitAbilityType findKey = (UnitAbilityType)i;

                    if (gearCore._status.Contains(findKey))
                        continue;

                    gearCore._status.Add((UnitAbilityType)i, new GearAbility());
                    break;
                }
            }

            // Key-Value Pair¸¦ Ç¥½Ã
            foreach (var kvp in gearCore._status.ToDictionary())
            {
                EditorGUILayout.LabelField($"[{kvp.Key.ToString()}]: Point {kvp.Value._point}, Percent {kvp.Value._percent * 100}%");
            }
        }
    }
}
