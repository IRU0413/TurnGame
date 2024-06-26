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
        [SerializeField] public SerializableDictionary<GearAbilityType, GearAbility> _abilities = new SerializableDictionary<GearAbilityType, GearAbility>();

        public GearType GearType => _gearType;
        public SerializableDictionary<GearAbilityType, GearAbility> Abillities => _abilities;

        protected override bool SettingData()
        {
            // 데이터 로드
            var so = GetSO<GearItemSO>();

            // 넣어주기
            _itemName = so.ItemName;
            _itemType = so.ItemType;
            _itemGradeType = so.ItemGradeType;

            _gearType = so.GearType;
            _abilities = so.Abilities;

            // 정상적으로 넣어졌는지 확인
            if (_gearType == GearType.None || _abilities.Count <= 0)
                return false;
            return true;
        }
        protected override bool SettingVisual()
        {
            bool isBaseVisualSetting = base.SettingVisual();
            if (!isBaseVisualSetting) return false;

            // 장비 위치에 배정.
            return true;
        }
    }
}
