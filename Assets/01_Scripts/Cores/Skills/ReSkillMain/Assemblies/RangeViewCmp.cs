using Scripts.Enums;
using Scripts.Manager;

namespace Scripts.Cores.Skills.ReSkillMain.Assemblies
{
    public class RangeViewCmp : SkillMainAssembly
    {
        private bool _isDraw = false;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _actionState = SkillState.Ready; // 메인 코어에서 Ready 상태일 때 실행 되겠금하는 것임
        }

        protected override void Action()
        {
            if (_actionState != SkillMain.State)
            {
                SkillManager.Instance.ClearRange(DrawLayerType.Base);
                SkillManager.Instance.ClearRange(DrawLayerType.Action);
                _isDraw = true;
            }
            else if (_isDraw)
            {
                SkillManager.Instance.DrawRange(DrawLayerType.Base, SkillMain.RangeList, SkillMain.Owner.Tr.position);
                SkillManager.Instance.DrawRange(DrawLayerType.Action, SkillMain.ActionRangeList, SkillMain.ActionPoint);
                _isDraw = false;
            }
        }

    }
}

