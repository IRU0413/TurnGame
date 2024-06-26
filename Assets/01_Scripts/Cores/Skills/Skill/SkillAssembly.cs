using Scripts.Enums;
using UnityEngine;

namespace Scripts.Cores.Skills.Skill
{
    public class SkillAssembly : Assembly
    {
        public SkillCore SkillCore => Base as SkillCore;

        // 부품 타입
        protected SkillAssemblyType _skillAssemblyType = SkillAssemblyType.None;
        [Header("= 실행 포인트(변경하지 마시요) =")]
        [SerializeField] protected AssemblyStateType _actionPointType = AssemblyStateType.None;

        // SkillCore에 추가할 때 키로 사용됨
        public SkillAssemblyType AssemblyType => _skillAssemblyType;

        protected override void OnInitialized()
        {
            if (_skillAssemblyType == SkillAssemblyType.None)
            {
                this.enabled = false;
                return;
            }
            base.OnInitialized();
        }

    }
}