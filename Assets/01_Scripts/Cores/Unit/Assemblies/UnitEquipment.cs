using UnityEngine;

namespace Scripts.Cores.Unit.Assemblies
{
    [DisallowMultipleComponent]
    public class UnitEquipment : UnitAssembly
    {
        private GearAbility _helmet;
        private GearAbility _innerTop;
        private GearAbility _outerTop;
        private GearAbility _bottom;
        private GearAbility _back;
        private GearAbility _hand;

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
    }
}