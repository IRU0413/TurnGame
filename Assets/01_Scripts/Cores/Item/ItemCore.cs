using Scripts.Enums;
using UnityEngine;

namespace Scripts.Cores.Item
{
    public abstract class ItemCore : Core
    {
        [SerializeField] private int _id;

        [SerializeField] private string _itemName; // 이름
        [SerializeField] private ItemType _itemType; // 아이템 타입
        [SerializeField] private Vector2Int _fieldPosition; // 필드 위치

        [SerializeField] private Sprite _icon;

        // 생성
        // -> 주인 없음
        // -> 필드에 생성(좌표 있음)
        public virtual void Spawn() { }

        // 획득
        // -> 그냥 획득
        // -> 인벤 있는 유닛인지 확인
        public virtual void GetItem() { }
        // 소멸
        // -> 그냥 소멸
    }
}
