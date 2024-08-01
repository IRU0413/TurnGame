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
            // 데이터 로드
            var so = GetSO<GearItemSO>();

            // 넣어주기
            _itemName = so.ItemName;
            _itemType = so.ItemType;
            _itemGradeType = so.ItemGradeType;
            _itemSprite = so.ItemSprite;
            _itemSpriteRotation = so.SpriteRotationCalibration;

            _gearType = so.GearType;
            _abilities = so.Abilities;

            // 아이템 이름 정의
            this.gameObject.name = $"Item_{_id}_{_itemName}";

            // 정상적으로 넣어졌는지 확인
            if (_gearType == GearType.None || _abilities.Count <= 0)
                return false;
            return true;
        }
    }
}
