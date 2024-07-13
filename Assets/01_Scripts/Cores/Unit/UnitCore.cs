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
        [SerializeField] private UnitStateType _state = UnitStateType.None; // 상태
        [SerializeField] private UnitAttackType _attackType = UnitAttackType.None;

        // 필수 컴포넌트 (아이디 필수 필요)
        protected UnitAnimator _ani; // 비주얼, 애니메이션
        protected UnitAbility _ability; // 능력치

        public int ID => _id;
        public UnitType UnitType => _unitType;
        public UnitStateType State => _state;
        public UnitAttackType AttackType => _attackType;

        public UnitAnimator Ani => _ani;
        public UnitAbility Ability => _ability;

        public void Spawn(int id)
        {
            // 로드 필요함
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
            this.LogPrint(">>>>>>>>>> Unit 초기화 완료");
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