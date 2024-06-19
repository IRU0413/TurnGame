using Scripts.Enums;

namespace Scripts.Cores.Skills.ReSkillMain
{
    public class SkillMainAssembly : Assembly
    {
        protected SkillMainCore SkillMain => BaseCore as SkillMainCore;
        protected SkillState _actionState = SkillState.None;

        // Not Need Base Fuction
        protected virtual void Action() { }

        private void Update()
        {
            if (!_isInitialized)
                return;
            Action();
        }
    }
}