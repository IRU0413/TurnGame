namespace Scripts.SO
{
    using Scripts.Cores.Unit;
    using Scripts.Enums;
    using Scripts.Pattern;
    using UnityEngine;

    [CreateAssetMenu(fileName = "GearItemSO", menuName = "ScriptableObjects/Item/Gear")]
    public class GearItemSO : ItemSO
    {
        [SerializeField] private GearType _gearType = GearType.None; // 장비 타입
        [SerializeField] private SerializableDictionary<UnitAbilityType, Ability> _abilities = new();

        public GearType GearType => _gearType;
        public SerializableDictionary<UnitAbilityType, Ability> Abilities => _abilities;

        // Generator에서 사용.
        public void SetData(int id, string itemName, ItemGradeType itemGradeType, Sprite[] itemSprite,
            GearType gearType, SerializableDictionary<UnitAbilityType, Ability> abilities)
        {
            _id = id;

            _itemName = itemName;
            _itemType = ItemType.Gear;
            _itemGradeType = itemGradeType;
            _itemSprite = itemSprite;

            _gearType = gearType;
            _abilities = abilities;
        }
    }
}