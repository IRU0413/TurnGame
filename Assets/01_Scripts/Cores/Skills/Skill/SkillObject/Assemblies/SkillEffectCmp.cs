using Scripts.Cores.Skills.Skill;
using UnityEngine;

namespace Scripts.Cores.Skills.SkillObject.Assemblies
{
    public class SkillEffectCmp : SkillAssembly
    {
        // ���� ���ֿ��� ���� ����Ʈ, X
        [SerializeField] private GameObject _usedEffectOfCastingUnit;
        // �ɷ� ���� ���ֿ��� ���� ����Ʈ
        [SerializeField] private GameObject _usedEffectOfAbilityApplyUnit;
        // �ɷ� ���� ���ֿ��� ���� ����Ʈ
        [SerializeField] private GameObject _usedEffectOfCastingPoint;

        public GameObject EffectCastingUnit => _usedEffectOfCastingUnit;
        public GameObject EffectCastingPoint => _usedEffectOfCastingPoint;
        public GameObject EffectAbilityApplyUnit => _usedEffectOfAbilityApplyUnit;

        protected override void OnInitialized()
        {

        }
    }
}
