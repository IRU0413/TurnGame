namespace Scripts.SO
{
    using Scripts.Cores.Unit;
    using Scripts.Enums;
    using Scripts.Pattern;
    using UnityEngine;

    [CreateAssetMenu(fileName = "GearItemSO", menuName = "ScriptableObjects/Item/Gear")]
    public class GearItemSO : ItemSO
    {
        [SerializeField] private GearType _gearType = GearType.None; // ��� Ÿ��
        [SerializeField] private SerializableDictionary<GearAbilityType, GearAbility> _abilities = new();

        public GearType GearType => _gearType;
        public SerializableDictionary<GearAbilityType, GearAbility> Abilities => _abilities;

        // Generator���� ���.
        public void SetData(int id, string itemName, ItemGradeType gradeType,
            GearType gearType, SerializableDictionary<GearAbilityType, GearAbility> abilities)
        {
            _id = id;
            _itemName = itemName;
            _itemType = ItemType.Gear;
            _itemGradeType = gradeType;

            _gearType = gearType;
            _abilities = abilities;
        }
    }
}