using Scripts.Cores.Skills.SkillObject;

namespace Scripts.Cores.Skills.Skill
{
    public class SkillObjectAssembly : Assembly
    {
        public SkillObjectCore Core => BaseCore as SkillObjectCore;
    }

}