using Scripts.Cores.Unit;
using Scripts.Enums;
using Scripts.Pattern;
using UnityEngine;

namespace Scripts.Cores.Item.Gear
{
    public class GearItemCore : ItemCore
    {
        [SerializeField] private GearType _gearType = GearType.None;
        [SerializeField] public SerializableDictionary<UnitAbilityType, GearAbility> _status = new SerializableDictionary<UnitAbilityType, GearAbility>();

    }
}
