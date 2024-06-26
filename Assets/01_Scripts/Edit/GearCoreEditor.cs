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

            if (gearCore._abilities == null)
            {
                gearCore._abilities = new SerializableDictionary<GearAbilityType, GearAbility>();
            }

            if (GUILayout.Button("Add Entry"))
            {
                int start = (int)GearAbilityType.MaxHealth;
                int end = (int)GearAbilityType.ActionSpeed;
                for (int i = start; i <= end; i++)
                {
                    GearAbilityType findKey = (GearAbilityType)i;

                    if (gearCore._abilities.Contains(findKey))
                        continue;

                    gearCore._abilities.Add((GearAbilityType)i, new GearAbility());
                    break;
                }
            }

            // Key-Value Pair¸¦ Ç¥½Ã
            foreach (var kvp in gearCore._abilities.ToDictionary())
            {
                EditorGUILayout.LabelField($"[{kvp.Key.ToString()}]: Point {kvp.Value.Point}, Percent {kvp.Value.Percent * 100}%");
            }
        }
    }
}
