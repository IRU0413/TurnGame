using Scripts.Cores.Item.Gear;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Cores.Unit.Assemblies
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UnitInventory))]
    public class UnitEquipment : UnitAssembly
    {
        private GearItemCore _helmet;
        private GearItemCore _outerTop;
        private GearItemCore _innerTop;
        private GearItemCore _bottom;
        private GearItemCore _back;
        private GearItemCore _weapon;
        private GearItemCore _shield;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        // ����
        public void Equip(GearItemCore gear)
        {
            // �����Ϸ��� �κп� ���� ��� �ִٸ� ���� ��� �����Ѵ�.
            GearItemCore previouslyEquippedGear = GetGear(gear.GearType);
            if (previouslyEquippedGear != null)
            {
                if (previouslyEquippedGear == gear)
                {
                    return;
                }
                Unequip(gear.GearType);
            }

            // ��� ����
            SetGear(gear.GearType, gear);

            // �κ��丮�� �־������� ��
            if (gear.IsInInventory)
            {
                var inven = Unit.GetAssembly<UnitInventory>();
                if (inven != null)
                {
                    inven.RemoveItem(gear);
                }
            }
        }
        // ����
        public void Unequip(GearType type)
        {
            GearItemCore gear = GetGear(type);
            if (gear == null) return;

            // ����
            SetGear(gear.GearType, null);
            var inven = Unit.GetAssembly<UnitInventory>();
            if (inven != null)
                inven.AddItem(gear);
        }

        private GearItemCore GetGear(GearType type)
        {
            GearItemCore unequipGear = null;
            switch (type)
            {
                case GearType.Helmet:
                    unequipGear = _helmet;
                    break;
                case GearType.InnerTop:
                    unequipGear = _innerTop;
                    break;
                case GearType.OuterTop:
                    unequipGear = _outerTop;
                    break;
                case GearType.Bottom:
                    unequipGear = _bottom;
                    break;
                case GearType.Back:
                    unequipGear = _back;
                    break;
                case GearType.Weapon:
                    unequipGear = _weapon;
                    break;
                case GearType.Shield:
                    unequipGear = _shield;
                    break;
                default:
                    Debug.LogError("��� ������ ���� ���� �� ���� ��� Ÿ���Դϴ�.");
                    break;
            }

            return unequipGear;
        }
        private void SetGear(GearType type, GearItemCore gear)
        {
            if (type == GearType.None && gear != null)
            {
                Debug.LogError("�־��� �� �����ϴ�.");
                return;
            }

            switch (type)
            {
                case GearType.Helmet:
                    _helmet = gear;
                    break;
                case GearType.InnerTop:
                    _innerTop = gear;
                    break;
                case GearType.OuterTop:
                    _outerTop = gear;
                    break;
                case GearType.Bottom:
                    _bottom = gear;
                    break;
                case GearType.Back:
                    _back = gear;
                    break;
                case GearType.Weapon:
                    _weapon = gear;
                    break;
                case GearType.Shield:
                    _shield = gear;
                    break;
                default:
                    Debug.LogError("�ش� ��� �������� Ÿ���� ������ �����Դϴ�.");
                    return;
            }
        }
    }
}