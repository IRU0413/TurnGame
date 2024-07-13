using Scripts.Cores.Unit.Assemblies;
using Scripts.Enums;
using Scripts.Manager;
using Scripts.Util;
using UnityEngine;

namespace Scripts.Cores.Unit
{
    public class UnitCore : Core
    {
        [Header("= UNIT BASE =")]
        [SerializeField] private int _id = -1;
        [SerializeField] private UnitType _unitType;
        [SerializeField] private UnitStateType _state = UnitStateType.None; // ����
        [SerializeField] private UnitAttackType _attackType = UnitAttackType.None;

        // �ʼ� ������Ʈ (���̵� �ʼ� �ʿ�)
        protected UnitAnimator _ani; // ���־�, �ִϸ��̼�
        protected UnitAbility _ability; // �ɷ�ġ

        public int ID => _id;
        public UnitType UnitType => _unitType;
        public UnitStateType State => _state;
        public UnitAttackType AttackType => _attackType;

        public UnitAnimator Ani => _ani;
        public UnitAbility Ability => _ability;

        public void Spawn(int id)
        {
            // �ε� �ʿ���
            _id = id;
            Initialized();
        }

        protected override void Initialized()
        {
            base.Initialized();
            _ani = GetOrAddAssembly<UnitAnimator>();
            _ability = GetOrAddAssembly<UnitAbility>();

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