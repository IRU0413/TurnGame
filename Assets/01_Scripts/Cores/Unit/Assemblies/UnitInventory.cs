using Scripts.Cores.Item;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cores.Unit.Assemblies
{
    [DisallowMultipleComponent]
    public class UnitInventory : UnitAssembly
    {
        private int _haveItemCount;
        [SerializeField] private int _haveItemMaxCount;
        [SerializeField] private List<ItemCore> _itemList = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetInventoryMaxCount(_haveItemMaxCount);
        }

        public void SetInventoryMaxCount(int count)
        {
            // ������ ������ְ�
            if (_itemList == null)
                _itemList = new List<ItemCore>();

            // �̻��� ���̶�� ����
            if (count <= 0)
                return;

            if (_haveItemCount > count)
            {

                for (int i = _itemList.Count - 1; i >= count; i--)
                {
                    var item = _itemList[i];
                    RemoveItem(item);
                }
            }

            _haveItemMaxCount = count;
        }

        public void AddItem(ItemCore item)
        {
            if (_haveItemCount >= _haveItemMaxCount)
            {
                Debug.LogWarning("�κ��丮�� �ִ�ġ �Դϴ�.");
                return;
            }

            if (!_itemList.Contains(item))
            {
                _itemList.Add(item);
                item.VisualGO.SetActive(false);
                _haveItemCount++;
            }
        }
        public void RemoveItem(ItemCore item)
        {
            if (!_itemList.Contains(item))
            {
                Debug.LogWarning("�κ��丮�� ���� �������� ���� �Ϸ� �õ��մϴ�.");
                return;
            }

            _itemList.Remove(item);
            item.VisualGO.SetActive(true);
            _haveItemCount--;
        }
    }
}