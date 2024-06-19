using Scripts.Cores.Skills.Skill;
using UnityEngine;

namespace Scripts.Cores.Skills.SkillObject.Assemblies
{
    public class SkillEffectCmp : SkillAssembly
    {
        // 시전 유닛에게 사용될 이펙트, X
        [SerializeField] private GameObject _usedEffectOfCastingUnit;
        // 능력 적용 유닛에게 사용될 이펙트
        [SerializeField] private GameObject _usedEffectOfAbilityApplyUnit;
        // 능력 적용 유닛에게 사용될 이펙트
        [SerializeField] private GameObject _usedEffectOfCastingPoint;

        public GameObject EffectCastingUnit => _usedEffectOfCastingUnit;
        public GameObject EffectCastingPoint => _usedEffectOfCastingPoint;
        public GameObject EffectAbilityApplyUnit => _usedEffectOfAbilityApplyUnit;

        protected override void OnInitialized()
        {

        }
    }
}
