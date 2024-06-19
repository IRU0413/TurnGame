using UnityEngine;

namespace Scripts.Cores.Skills.Skill
{
    public class SkillAudioCmp : SkillAssembly
    {
        [Header("= 스킬 오디오 필수 설정 =")]
        [SerializeField] private AudioClip _startSound; // 시작
        [SerializeField] private AudioClip _doingSound; // 실행 중
        [SerializeField] private AudioClip _endSound; // 끝

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}

