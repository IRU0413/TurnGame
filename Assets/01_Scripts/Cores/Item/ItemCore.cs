using Scripts.Enums;
using UnityEngine;

namespace Scripts.Cores.Item
{
    public abstract class ItemCore : Core
    {
        [SerializeField] private int _id;

        [SerializeField] private string _itemName; // �̸�
        [SerializeField] private ItemType _itemType; // ������ Ÿ��
        [SerializeField] private Vector2Int _fieldPosition; // �ʵ� ��ġ

        [SerializeField] private Sprite _icon;

        // ����
        // -> ���� ����
        // -> �ʵ忡 ����(��ǥ ����)
        public virtual void Spawn() { }

        // ȹ��
        // -> �׳� ȹ��
        // -> �κ� �ִ� �������� Ȯ��
        public virtual void GetItem() { }
        // �Ҹ�
        // -> �׳� �Ҹ�
    }
}
