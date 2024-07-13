using Scripts.Enums;

namespace Scripts.Cores.Skills.ReSkillMain
{
    public class SkillActionAssembly : Assembly
    {
        protected SkillActionCore SkillMain => Base as SkillActionCore;
        protected SkillState _actionState = SkillState.None;

        // Not Need Base Fuction
        protected virtual void OnAction() { }

        private void Update()
        {
            if (!IsInitialized)
                return;
            OnAction();
        }
    }
}