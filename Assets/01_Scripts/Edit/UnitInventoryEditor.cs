using Scripts.Cores.Item;
using Scripts.Cores.Unit.Assemblies;
using UnityEditor;
using UnityEngine;

namespace Scripts.Edit
{
    [CustomEditor(typeof(UnitInventory))]
    public class UnitInventoryEditor : Editor
    {
        private ItemCore _targetItem;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UnitInventory inventory = (UnitInventory)target;
            if (!inventory.IsInitialized)
                return;

            _targetItem = EditorGUILayout.ObjectField("Target Item", _targetItem, typeof(ItemCore), true) as ItemCore;

            if (GUILayout.Button("Add Item"))
            {
                AddItem(inventory);
            }
            if (GUILayout.Button("Remove Item"))
            {
                RemoveItem(inventory);
            }
        }

        private void AddItem(UnitInventory inventory)
        {
            if (_targetItem != null)
            {
                ItemCore addItem = _targetItem.GetComponent<ItemCore>();

                if (addItem != null)
                {
                    inventory.AddItem(addItem);
                    EditorUtility.SetDirty(target);
                }

                _targetItem = null;
            }
        }
        private void RemoveItem(UnitInventory inventory)
        {
            if (_targetItem != null)
            {
                ItemCore removeItem = _targetItem.GetComponent<ItemCore>();

                if (removeItem != null)
                {
                    inventory.RemoveItem(removeItem);
                    EditorUtility.SetDirty(target);
                }

                _targetItem = null;
            }
        }
    }
}
