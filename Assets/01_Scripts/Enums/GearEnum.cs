namespace Scripts.Enums
{
    public enum GearType
    {
        None, // 미정
        Helmet, // 헬멧
        InnerTop, // 내피
        OuterTop, // 외피
        Bottom, // 하의
        Back, // 등
        Weapon, // 무기
        Shield, // 방패
    }

    public enum GearAbilityDataProperty
    {
        Index,

        Id,

        ItemName,
        ItemGradeType,

        GearType,

        Health_Point,
        Health_Percent,

        Mana_Point,
        Mana_Percent,

        MaxHealth_Point,
        MaxHealth_Percent,

        MaxMana_Point,
        MaxMana_Percent,

        AttackPower_Point,
        AttackPower_Percent,

        MagicPower_Point,
        MagicPower_Percent,

        Armor_Point,
        Armor_Percent,

        MagicResistance_Point,
        MagicResistance_Percent,

        CriticalStrikeChance_Point,
        CriticalStrikeChance_Percent,

        CriticalStrikeDamage_Point,
        CriticalStrikeDamage_Percent,

        ArmorPenetration_Point,
        ArmorPenetration_Percent,

        MagicPenetration_Point,
        MagicPenetration_Percent,

        ActionSpeed_Point,
        ActionSpeed_Percent
    }

    public enum GearAbilityType
    {
        MaxHealth,
        MaxMana,

        AttackPower,
        MagicPower,

        Armor,
        MagicResistance,

        CriticalStrikeChance,
        CriticalStrikeDamage,

        ArmorPenetration,
        MagicPenetration,

        ActionSpeed,
    }
}