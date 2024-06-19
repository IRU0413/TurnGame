using UnityEngine;

namespace Scripts.Cores.Skills.Skill
{
    public class SkillVisualGroupCmp : SkillAssembly
    {
        [SerializeField] private bool _isVisualationIcon; // 아이콘 시각화 여부
        [SerializeField] private bool _isVisualationAnimation; // 애니메이션 시각화 여부
        [SerializeField] private bool _isVisualationShadow; // 그림자 시각화 여부

        private GameObject _visualGroupGo = null;
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}

