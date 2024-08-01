namespace Scripts.SO
{
    using Scripts.Enums;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "UnitAbilitySO", menuName = "ScriptableObjects/Unit/Ability")]
    public class UnitAbilitySO : ScriptableObject
    {
        [HideInInspector] public float Id;

        public float Health;
        public float Mana;

        public float AttackPower;
        public float MagicPower;

        public float Armor;
        public float MagicResistance;

        public float CriticalStrikeChance;
        public float CriticalStrikeDamage;

        public float ArmorPenetration;
        public float MagicPenetration;

        public float ActionSpeed;

        public int MovePoint;

        public float GetAbility(UnitAbilityType abilityType)
        {
            return abilityType switch
            {
                UnitAbilityType.Health => Health,
                UnitAbilityType.Mana => Mana,
                UnitAbilityType.MaxHealth => Health,
                UnitAbilityType.MaxMana => Mana,
                UnitAbilityType.AttackPower => AttackPower,
                UnitAbilityType.MagicPower => MagicPower,
                UnitAbilityType.Armor => Armor,
                UnitAbilityType.MagicResistance => MagicResistance,
                UnitAbilityType.CriticalStrikeChance => CriticalStrikeChance,
                UnitAbilityType.CriticalStrikeDamage => CriticalStrikeDamage,
                UnitAbilityType.ArmorPenetration => ArmorPenetration,
                UnitAbilityType.MagicPenetration => MagicPenetration,
                UnitAbilityType.ActionSpeed => ActionSpeed,
                _ => -1
            };
        }

        public Dictionary<UnitAbilityType, float> GetCurrentAbility()
        {
            var abilities = new Dictionary<UnitAbilityType, float>();
            foreach (UnitAbilityType abilityType in Enum.GetValues(typeof(UnitAbilityType)))
            {
                abilities[abilityType] = GetAbility(abilityType);
            }
            return abilities;
        }
    }
}
