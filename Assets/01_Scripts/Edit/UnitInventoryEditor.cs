using Scripts.Cores.Item;
using Scripts.Cores.Unit.Assemblies;
using UnityEditor;
using UnityEngine;

namespace Scripts.Edit
{
    [CustomEditor(typeof(UnitInventory))]
    public class UnitInventoryEditor : Editor
    {
        private ItemCore _addItem;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UnitInventory inventory = (UnitInventory)target;
            if (!inventory.IsInitialized)
                return;

            _addItem = EditorGUILayout.ObjectField("Add Item", _addItem, typeof(ItemCore), true) as ItemCore;

            if (GUILayout.Button("Add Item"))
            {
                AddItem();
            }
        }

        private void AddItem()
        {
            UnitInventory inventory = (UnitInventory)target;

            // �ʵ忡�� �������� �巡�� �� ����� �� �ִ� ���� �߰�
            EditorGUILayout.HelpBox("Drag and drop an item GameObject here to add it to the inventory.", MessageType.Info);

            if (_addItem != null)
            {
                ItemCore addItem = _addItem.GetComponent<ItemCore>();

                if (addItem != null)
                {
                    inventory.AddItem(addItem);
                    EditorUtility.SetDirty(target);
                }

                _addItem = null;
            }
        }
    }
}
