using Scripts.Cores.Unit.Assemblies;
using Scripts.Enums;
using Scripts.Manager;
using Scripts.SO;
using Scripts.Util;
using UnityEngine;

namespace Scripts.Cores.Unit
{
    public class UnitCore : Core
    {
        [Header("= UNIT BASE =")]
        [SerializeField] private UnitType _unitType;
        [SerializeField] private UnitStateType _state = UnitStateType.None; // ����
        [SerializeField] private UnitAttackType _attackType = UnitAttackType.None;

        private UnitDataSO _data;
        // �ʼ� ������Ʈ (���̵� �ʼ� �ʿ�)
        protected UnitAnimator _ani; // ���־�, �ִϸ��̼�
        protected UnitAbility _ability; // �ɷ�ġ

        public int ID => _data.Id;
        public UnitType UnitType => _unitType;
        public UnitStateType State => _state;
        public UnitAttackType AttackType => _attackType;

        public UnitAbilitySO BaseAbility => _data.UnitAbilitySO;

        public UnitAnimator Ani => _ani;

        public void Spawn(int id)
        {
            _data = GameManager.Resource.Load<UnitDataSO>($"SO/Unit/Data/UnitData_{id}");
            Initialized();
        }

        protected override void Initialized()
        {
            // �ʿ� ������
            if (_data == null)
                _data = GameManager.Resource.Load<UnitDataSO>($"SO/Unit/Data/UnitData_{10000}");
            base.Initialized();

            // ��� ���� ����
            // _ani = GetOrAddAssembly<UnitAnimator>();
            // _ability = GetOrAddAssembly<UnitAbility>();

            _state = UnitStateType.Idle;
            UnitManager.Instance.Add(this);
            this.LogPrint(">>>>>>>>>> Unit �ʱ�ȭ �Ϸ�");
        }

        public virtual void SetState(UnitStateType state)
        {
            if (state == UnitStateType.None) return;
            if (_ani == null) return;

            _state = state;
            _ani.PlayAnimation(_state, _attackType);
        }
        public virtual void SetAttackType(UnitAttackType attackType)
        {
            _attackType = attackType;
        }

        protected virtual void Start()
        {
            // Test
            if (GameManager.GameMode == GameModeType.Test)
                Initialized();
        }
    }
}