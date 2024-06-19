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
        None, // 없음
        Search, // 탐색(= 예상) 범위
        Active, // 적용 범위
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
        ALL, // 범위 내 모든 유닛
        UseUnit, // 사용 유닛만
        Friendly, // 우호적인 유닛
        Hostile, // 적대적인 유닛
    }

    public enum SkillTargetType
    {
        UseUnit, // 시전자
        Unit, // 유닛
        Tile, // 타일
    }
    // 스킬ㄴ
    public enum SkillControlStateType
    {
        None, // ... / 스킬 부품에서는 단계 상관 없이 돌아가는 것을 의미
        Ready, // 스킬 실행 준비 단계
        Choose, // 스킬 선택 단계
        Action, // 스킬 활성화(= 진행 단계)
        End,
    }

    public enum SkillState
    {
        None, // 가동 상태 아님
        Ready, // 준비(선택된 상태)
        Action, // 실행
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
        None, // 비활성화
        Base, // 세팅 코어
        Action, // 실행 오브젝트 코어
    }
}