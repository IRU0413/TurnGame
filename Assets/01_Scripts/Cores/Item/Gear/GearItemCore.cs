using Scripts.Cores.Unit;
using Scripts.Enums;
using Scripts.Pattern;
using Scripts.SO;
using UnityEngine;

namespace Scripts.Cores.Item.Gear
{
    public class GearItemCore : ItemCore
    {
        [SerializeField] private GearType _gearType = GearType.None;
        [SerializeField] private SerializableDictionary<UnitAbilityType, Ability> _abilities = new();

        public GearType GearType => _gearType;
        public SerializableDictionary<UnitAbilityType, Ability> Abillities => _abilities;
        public float GearSpriteRotation => _itemSpriteRotation;

        protected override bool SettingData()
        {
            // ������ �ε�
            var so = GetSO<GearItemSO>();

            // �־��ֱ�
            _itemName = so.ItemName;
            _itemType = so.ItemType;
            _itemGradeType = so.ItemGradeType;
            _itemSprite = so.ItemSprite;
            _itemSpriteRotation = so.SpriteRotationCalibration;

            _gearType = so.GearType;
            _abilities = so.Abilities;

            // ������ �̸� ����
            this.gameObject.name = $"Item_{_id}_{_itemName}";

            // ���������� �־������� Ȯ��
            if (_gearType == GearType.None || _abilities.Count <= 0)
                return false;
            return true;
        }
    }
}
