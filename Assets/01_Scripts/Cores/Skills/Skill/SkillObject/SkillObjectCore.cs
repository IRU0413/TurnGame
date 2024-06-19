using Scripts.Cores.Skills.Skill;
using Scripts.Cores.Skills.Skill.SkillMain;
using Scripts.Enums;
using UnityEngine;

namespace Scripts.Cores.Skills.SkillObject
{
    public class SkillObjectCore : SkillCore
    {
        private SkillMainCore _root;
        private bool _isAction = false;

        [Header("= 스킬 옵젝 설정 =")]
        [SerializeField] private Vector2 _offsetVector = Vector2.zero;

        public void Init(SkillMainCore root)
        {
            _root = root;
            if (_root == null) return;

            Transform parentTr = root.Pocket.transform;
            if (parentTr == null) return;

            Initialized();

            Tr.parent = parentTr;
        }
        protected override void Initialized()
        {
            CoreType = DrawLayerType.Base;
            base.Initialized();
        }

        #region Draw
        public void DrawRange(Vector2 point)
        {
            base.SkillDrawRange(DrawLayerType.Action, point);
        }
        public void ClearDrawRange()
        {
            base.ClearSkillDrawRange(DrawLayerType.Action);
        }

        #endregion

        protected override void StateUpdate()
        {
        }
    }
}
