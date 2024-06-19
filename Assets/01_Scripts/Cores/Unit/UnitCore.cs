using Scripts.Cores.Unit.Assemblies;
using Scripts.Enums;
using Scripts.Manager;
using Scripts.Util;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scripts.Cores.Unit
{
    [RequireComponent(typeof(UnitAnimationCtrlCmp))]
    [RequireComponent(typeof(UnitEquipment))]
    public class UnitCore : Core
    {
        [Header("= UNIT BASE =")]
        [SerializeField] private UnitStateType _state = UnitStateType.None; // 상태
        [SerializeField] private UnitAttackType _attackType = UnitAttackType.None;
        [SerializeField] private bool _isTest = false;

        private SortingGroup _sortingLayer = null;

        // Controller Cmp
        private UnitAnimationCtrlCmp _aniCtrl = null;
        private UnitEquipment gearCtrl = null;

        protected override void Initialized()
        {
            base.Initialized();
            UnitManager.Instance.Add(this);
            if (!UnitManager.Instance.ContainsUnit(this))
                return;

            _aniCtrl = this.GetAssembly<UnitAnimationCtrlCmp>();
            _state = UnitStateType.Idle;

            this.LogPrint(">>>>>>>>>> Unit 초기화 완료");
        }

        public void SetState(UnitStateType state)
        {
            if (state == UnitStateType.None) return;
            if (_aniCtrl == null) return;

            _state = state;
            _aniCtrl.PlayAnimation(_state, _attackType);
        }
        public void SetAttackType(UnitAttackType attackType)
        {
            _attackType = attackType;
        }

        protected virtual void Start()
        {
            Initialized();
        }
        protected virtual void Update()
        {
            if (!_isTest)
                return;
            TestInput();
        }
        private void TestInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetState(UnitStateType.Idle);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetState(UnitStateType.Move);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetAttackType(UnitAttackType.None);
                SetState(UnitStateType.Attack);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetState(UnitStateType.CC);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetState(UnitStateType.Die);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SetState(UnitStateType.Recover);
            }


            else if (Input.GetKeyDown(KeyCode.Q))
            {
                SetAttackType(UnitAttackType.NormalAttack);
                SetState(UnitStateType.Attack);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {

                SetAttackType(UnitAttackType.SwordAttack);
                SetState(UnitStateType.Attack);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                SetAttackType(UnitAttackType.MagicAttack);
                SetState(UnitStateType.Attack);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                SetAttackType(UnitAttackType.BowAttack);
                SetState(UnitStateType.Attack);
            }
        }
    }
}