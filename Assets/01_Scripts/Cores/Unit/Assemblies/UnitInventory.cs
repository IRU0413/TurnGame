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
            // 없으면 만들어주고
            if (_itemList == null)
                _itemList = new List<ItemCore>();

            // 이상한 값이라면 리턴
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
                Debug.LogWarning("인벤토리가 최대치 입니다.");
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
                Debug.LogWarning("인벤토리가 없는 아이템을 제거 하려 시도합니다.");
                return;
            }

            _itemList.Remove(item);
            item.VisualGO.SetActive(true);
            _haveItemCount--;
        }
    }
}