namespace Scripts.Enums
{
    public enum UnitProperty
    {
        Index,

        Name,

        JobType,

        Health,
        Mana,

        AttackPower,
        MagicPower,

        Armor,
        MagicResistacne,

        CriticalStrikeChance,
        CriticalStrikeDamage,

        ArmorPenetration,
        MagicPenetration,

        ActionSpeed,
    }

    public enum ActionUnitProperty
    {
        ID,

        Name,

        JobType,

        Health,
        Mana,

        AttackPower,
        MagicPower,

        Armor,
        MagicResistacne,

        CriticalStrikeChance,
        CriticalStrikeDamage,

        ArmorPenetration,
        MagicPenetration,

        ActionSpeed,

        MovePoint,

    }

    public enum UnitType
    {
        Player,
        Monster,
        NPC,
        Object,
    }

    // 스킬에 달아서 사용할 거임
    public enum UnitAttackType
    {
        None,
        NormalAttack, // 기본
        SwordAttack, // 검
        MagicAttack, // 마법
        BowAttack, // 활
    }

    public enum JobType
    {
        None,
        Warrior, // 전사
        Mage, // 마법사
        Rogue, // 도적
        Ranger, // 레인저
        Monk, // 몽크

        NormalMonster,
        EliteMonster,
        BossMonster,
    }

    public enum UnitAbilityType
    {
        Health,
        Mana,
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

    public enum UnitStateType
    {
        None,
        Idle, // 평범한 상태
        Move, // 이동
        Attack, // 행동(공격이나 버프 같은 애니메이션 같은 거 실행 할 때는 정수형 변수 하나 뚤어서 사용)
        CC,
        Die, // 죽은 상태
        Recover, // 회복(부활)
        Hit, // 죽은 상태
    }

    public enum ActionType
    {
        None,
        Move,
        Ability,
    }
}
