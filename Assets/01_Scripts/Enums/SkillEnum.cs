namespace Scripts.Enums
{
    public enum AbilityType
    {
        None,
        Weapon,
        Passive,
        Action,
    }

    public enum ApplicableDecision
    {
        Melee,
        Ranged,
        Magic,
    }

    public enum SkillProperty
    {
        Index,

        ID,

        AbilityType,

        Name,

        Mana,
        CoolTime,

        SearchFormat,
        DrawPoint,
        SearchDistance,

        MinEffectApplyUnitCount,
        MaxEffectApplyUnitCount,

        Icon,
        SkillGo,
        SkillActiveSound,
    }

    public enum SkillApplyStatus
    {
        Health, // MaxHealth
        Mana, // MaxMana

        AttackPower,
        MagicPower,

        Armor,
        MagicResistacne,

        CriticalStrikeChance,
        CriticalStrikeDamage,

        ArmorPenetration,
        MagicPenetration,

        ActiveSpeed,
    }

    public enum SkillDamageType
    {
        Normal,
        Fixed,
    }

    public enum SkillRangeType
    {
        None, // ����
        Search, // Ž��(= ����) ����
        Active, // ���� ����
    }

    public enum ActionPointCorrection
    {
        Center,
        Up,
        Right,
        Down,
        Left,
    }

    public enum SkillApplyUnitType
    {
        ALL, // ���� �� ��� ����
        UseUnit, // ��� ���ָ�
        Friendly, // ��ȣ���� ����
        Hostile, // �������� ����
    }

    public enum SkillTargetType
    {
        UseUnit, // ������
        Unit, // ����
        Tile, // Ÿ��
    }
    // ��ų��
    public enum SkillControlStateType
    {
        None, // ... / ��ų ��ǰ������ �ܰ� ��� ���� ���ư��� ���� �ǹ�
        Ready, // ��ų ���� �غ� �ܰ�
        Choose, // ��ų ���� �ܰ�
        Action, // ��ų Ȱ��ȭ(= ���� �ܰ�)
        End,
    }

    public enum SkillState
    {
        None, // ���� ���� �ƴ�
        Ready, // �غ�(���õ� ����)
        Action, // ����
        End,
    }

    public enum SkillAssemblyType
    {
        None,
        SkillAudio,
        SkillActivationCondition,
        SkillVisualGroup,
    }

    public enum DrawLayerType
    {
        None, // ��Ȱ��ȭ
        Base, // ���� �ھ�
        Action, // ���� ������Ʈ �ھ�
    }
}