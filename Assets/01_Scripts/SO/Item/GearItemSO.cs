namespace Scripts.SO
{
    using Scripts.Cores.Unit;
    using Scripts.Enums;
    using Scripts.Pattern;
    using UnityEngine;

    [CreateAssetMenu(fileName = "GearItemSO", menuName = "ScriptableObjects/Item/Gear")]
    public class GearItemSO : ItemSO
    {
        public GearType GearType = GearType.None; // ��� Ÿ��
        public SerializableDictionary<UnitAbilityType, Ability> Abilities = new();
    }
}