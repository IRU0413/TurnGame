using Scripts.Cores.Skills.Skill;
using Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

public class SkillApplyAbilityStatus : SkillObjectAssembly
{
    [System.Serializable]
    public struct AbilityStatus
    {
        [SerializeField] private SkillApplyUnitType _applyUnitType;
        [SerializeField] private SkillApplyStatus _applyAbilityStatus; // 적용 능력 능력치
        [SerializeField] private float _abilityStatusValue;
        [SerializeField] private float _abilityStatusPercent;

        public SkillApplyStatus Status => _applyAbilityStatus;
        public float Value => _abilityStatusValue;
        public float Percent => _abilityStatusPercent;
    }

    [SerializeField] private List<AbilityStatus> _abilities;

    public List<AbilityStatus> Abilities => _abilities;
}
