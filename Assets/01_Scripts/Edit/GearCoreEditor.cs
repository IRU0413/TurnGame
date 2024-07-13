using Scripts.Cores.Item.Gear;
using UnityEditor;

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

            /*if (gearCore._abilities == null)
            {
                gearCore._abilities = new SerializableDictionary<GearAbilityType, Ability>();
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

                    gearCore._abilities.Add((GearAbilityType)i, new Ability());
                    break;
                }
            }*/

            // Key-Value Pair¸¦ Ç¥½Ã
            foreach (var kvp in gearCore.Abillities.ToDictionary())
            {
                EditorGUILayout.LabelField($"[{kvp.Key}]: Point {kvp.Value.Point}, Percent {kvp.Value.Percent * 100}%");
            }
        }
    }
}
