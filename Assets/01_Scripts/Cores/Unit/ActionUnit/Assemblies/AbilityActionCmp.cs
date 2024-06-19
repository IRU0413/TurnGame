using Scripts.Cores.Skills.ReSkillMain;
using Scripts.Cores.Unit.ActinoUnit.InsideAssemblies;
using Scripts.Enums;
using Scripts.Manager;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cores.Unit.ActinoUnit.Assemblies
{
    [DisallowMultipleComponent]
    public class AbilityActionCmp : ActionAssembly
    {
        // ��� ���� ����
        [Header("= ���� =")]
        [SerializeField] private int _passiveSlot = 1;
        [SerializeField] private int _weaponSlot = 1;
        [SerializeField] private int _activeSlot = 2;

        [Header("= ��� ��ų ���� =")]
        [SerializeField] private SkillMainCore _useSkill; //  ��� ��ų

        private MouseInputType _mouseInputType = MouseInputType.None;
        private Vector2 _tilePoint = Vector2.zero; // ���콺 �Է��� ���ٰ� �Է��� ���� ����Ʈ
        private List<ActionUnitCore> _seletedUnits;

        private AbilityItemBox _abilityBox = new();

        public Vector2 TilePoint => _tilePoint;
        public SkillMainCore UseSkill
        {
            get => _useSkill;
            set
            {
                if (_useSkill != null)
                    _useSkill.State = SkillState.None;
                _useSkill = value;
                this.LogPrint($"��� ��ų ����: [{_useSkill}]");
            }
        }

        protected override void OnInitialized()
        {
            // itemBox �ʱ�ȭ
            if (_abilityBox == null)
                _abilityBox = new();

            _abilityBox.Init(ActionUnitCore.Tr, _weaponSlot, _passiveSlot, _activeSlot);

            // �ڽ� �ȿ� �ִ� ��ų �ھ���� ã�� �ʱ�ȭ ���� �� ������ ����
            var skills = ActionUnitCore.GetComponentsInChildren<SkillMainCore>();
            HaveSkillsSetOwner(skills);

            ActionAssemblyType = ActionType.Ability;
            base.OnInitialized();
        }
        private void HaveSkillsSetOwner(SkillMainCore[] skills)
        {
            foreach (var skill in skills)
            {
                skill.Create();
                skill.Owner = ActionUnitCore;
            }
        }

        #region Slot Add, Remove, Get
        public void AddAbility(SkillMainCore skill)
        {
            // �߰�
            _abilityBox.Add(skill);
        }
        public void RemoveAbility(SkillMainCore skill)
        {
            _abilityBox.Remove(skill);
        }
        public SkillMainCore GetAbility(AbilityType type, int idx)
        {
            SkillMainCore choose = _abilityBox.Select(type, idx);
            if (choose == null)
            {
                this.WarningPrint($"���õ� ��ų�� �����ϴ�.\n" +
                    $"Ability Box _isInitialized: {_abilityBox?.IsInitialized}\n" +
                    $"AbilityType: {type}\n" +
                    $"choose Index: {idx}\n");
            }

            return choose;
        }

        #endregion

        #region Input
        public override void KeyBoardAction(KeyboardInputType inputType)
        {
            base.KeyBoardAction(inputType);
            if ((this.State == AssemblyStateType.Ready || this.State == AssemblyStateType.Choose)
                && inputType == KeyboardInputType.Down)
            {
                if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))
                    UseSkill = GetAbility(AbilityType.Weapon, 0);
                else if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2))
                    UseSkill = GetAbility(AbilityType.Action, 0);
                else if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3))
                    UseSkill = GetAbility(AbilityType.Action, 1);
            }
        }
        public override void MouseAction(MouseInputType inputType, Vector3 mousePos)
        {
            // base.MouseAction(inputType, mousePos);
            _tilePoint = SkillManager.Instance.GetTilePointToPosition(mousePos);
            _mouseInputType = inputType;
        }

        #endregion

        #region Setting
        protected override void NoneStateSetting()
        {
            base.NoneStateSetting();
            _useSkill = null;
        }
        protected override void ReadyStateSetting()
        {
            base.ReadyStateSetting();
            _useSkill = null;
        }
        protected override void ChooseStateSetting()
        {
            base.ChooseStateSetting();
            if (_useSkill != null)
                _useSkill.State = SkillState.Ready;
        }
        protected override void ActionStateSetting()
        {
            base.ActionStateSetting();
            _useSkill.State = SkillState.Action;
            /*if (_useSkill == null)
            {
                this.LogPrint("OnAction �ܰ�� �Ѿ�Դµ� ��� �Ϸ��� ��ų�� �����ϴ�.");
                this.State = AssemblyStateType.Ready;
                return;
            }*/
        }
        protected override void EndStateSetting()
        {
            base.EndStateSetting();
            if (State == AssemblyStateType.End)
                State = AssemblyStateType.Ready;
        }

        #endregion

        #region Update
        protected override void ReadyUpdate()
        {
            base.ReadyUpdate();
            if (_useSkill != null)
                this.State = AssemblyStateType.Choose;
        }
        protected override void ChooseUpdate()
        {
            base.ChooseUpdate();
            if (_mouseInputType == MouseInputType.Click)
                this.State = AssemblyStateType.Action;
        }
        protected override void ActionUpdate()
        {
            base.ActionUpdate();
            if (UseSkill.State == SkillState.None)
                this.State = AssemblyStateType.End;
        }

        #endregion

    }
}