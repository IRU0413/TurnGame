using Scripts.Enums;
using Scripts.Pattern;
using UnityEngine;

namespace Scripts.Cores.Unit.Assemblies
{
    [DisallowMultipleComponent]
    public class UnitEquipment : UnitAssembly
    {
        private SerializableDictionary<UnitAbilityType, GearAbility> _helmet;
        private SerializableDictionary<UnitAbilityType, GearAbility> _innerTop;
        private SerializableDictionary<UnitAbilityType, GearAbility> _outerTop;
        private SerializableDictionary<UnitAbilityType, GearAbility> _bottom;
        private SerializableDictionary<UnitAbilityType, GearAbility> _back;
        private SerializableDictionary<UnitAbilityType, GearAbility> _hand;

        public GearAbility Helmet
        {
            get => _helmet;
            set => _helmet = value;
        }
        public GearAbility InnerTop
        {
            get => _innerTop;
            set => _innerTop = value;
        }
        public GearAbility OuterTop
        {
            get => _outerTop;
            set => _outerTop = value;
        }
        public GearAbility Bottom
        {
            get => _bottom;
            set => _bottom = value;
        }
        public GearAbility Back
        {
            get => _back;
            set => _back = value;
        }
        public GearAbility Hand
        {
            get => _hand;
            set => _hand = value;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        public void Equip(GearType gear, SerializableDictionary<UnitAbilityType, GearAbility> status)
        {
            if (gear == GearType.None)
            {
                Debug.LogError("해당 장비 아이템의 타입은 미지정 상태입니다.");
                return;
            }
            if (status == null)
            {
                Debug.LogError("해당 장비 아이템의 능력치가 Null입니다...");
                return;
            }

            switch (gear)
            {
                case GearType.Helmet:
                    _helmet = status;
                    break;
                case GearType.InnerTop:
                    _innerTop = status;
                    break;
                case GearType.OuterTop:
                    _outerTop = status;
                    break;
                case GearType.Bottom:
                    _bottom = status;
                    break;
                case GearType.Back:
                    _back = status;
                    break;
                case GearType.Hand:
                    _hand = status;
                    break;
            }
        }

        public void Unequip(GearType gear)
        {

        }
    }
}