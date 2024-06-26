using Scripts.Enums;
using UnityEngine;

namespace Scripts.Cores.Skills.Skill
{
    public class SkillAssembly : Assembly
    {
        public SkillCore SkillCore => Base as SkillCore;

        // ��ǰ Ÿ��
        protected SkillAssemblyType _skillAssemblyType = SkillAssemblyType.None;
        [Header("= ���� ����Ʈ(�������� ���ÿ�) =")]
        [SerializeField] protected AssemblyStateType _actionPointType = AssemblyStateType.None;

        // SkillCore�� �߰��� �� Ű�� ����
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