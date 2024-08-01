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

    // ��ų�� �޾Ƽ� ����� ����
    public enum UnitAttackType
    {
        None,
        NormalAttack, // �⺻
        SwordAttack, // ��
        MagicAttack, // ����
        BowAttack, // Ȱ
    }

    public enum JobType
    {
        None,
        Warrior, // ����
        Mage, // ������
        Rogue, // ����
        Ranger, // ������
        Monk, // ��ũ

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
        Idle, // ����� ����
        Move, // �̵�
        Attack, // �ൿ(�����̳� ���� ���� �ִϸ��̼� ���� �� ���� �� ���� ������ ���� �ϳ� �Ծ ���)
        CC,
        Die, // ���� ����
        Recover, // ȸ��(��Ȱ)
        Hit, // ���� ����
    }

    public enum ActionType
    {
        None,
        Move,
        Ability,
    }
}
