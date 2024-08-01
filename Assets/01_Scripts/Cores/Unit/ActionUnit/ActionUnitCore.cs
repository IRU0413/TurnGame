using Scripts.Data;
using Scripts.Enums;
using Scripts.Manager;
using Scripts.SO;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cores.Unit.ActinoUnit
{
    public abstract class ActionUnitCore : UnitCore
    {
        protected Dictionary<ActionType, ActionAssembly> _actionAssemblies = new();
        protected ActionUnitStatus _status = null;

        [Header("= ACTION_UNIT BASE =")]
        [SerializeField] protected int _ID = -1;
        protected string _unitName = "None";

        [SerializeField] protected JobType _jobType;
        [SerializeField] protected ActionType _actionType;

        [SerializeField] protected bool _isActionAble = false;
        [SerializeField] protected bool _isMyTurn = false;
        [SerializeField] protected float _actionPoint = 0;

        protected int _actionTurnCounting = 0;

        public ActionUnitStatus Status => _isInitialized ? _status : null;
        public JobType Job => _jobType;
        public ActionType Action
        {
            get => _actionType;
            set => ActionSetting(value);
        }

        public bool IsMyTurn => _isMyTurn;
        public bool IsActionAble
        {
            get => _isActionAble;
            set
            {
                if (value) ActionManager.Instance.Add(this);
                else ActionManager.Instance.Remove(this);
                _isActionAble = value;
            }
        }
        public float ActionPoint
        {
            get => _actionPoint;
            set => _actionPoint = value;
        }

        protected override void Initialized()
        {
            base.Initialized();

            _actionPoint = GetActionPoint();
            SetState(UnitStateType.Idle);
            _actionTurnCounting = 0;

            /*List<ActionAssembly> actions = GetAllAssembly<ActionAssembly>();
            foreach (var action in actions)
            {
                _actionAssemblies.Add(action.ActionAssemblyType, action);
            }*/
        }

        public void MyTurn()
        {
            _actionPoint = 0;
            _isMyTurn = true;
            _actionTurnCounting++;
        }

        public void EndTurn()
        {
            _actionPoint += GetActionPoint();
            _isMyTurn = false;
        }

        private float GetActionPoint()
        {
            return Mathf.Round((10000 / Status.CurrActionSpeed) * 100) / 100;
        }

        private void ActionSetting(ActionType action)
        {
            if (action == ActionType.None && _actionType != ActionType.None)
            {
                _actionAssemblies[_actionType].TakeAction = false;
            }
            else if (_actionType == ActionType.None)
            {
                _actionAssemblies[action].TakeAction = true;
            }
            else
            {
                return;
            }
            _actionType = action;
        }

        // 외부에서 데이터를 로드에서 넣어줬을 경우임.
        public void Create(UnitDataSO so = null)
        {
            if (IsInitialized) return;

            if (so == null)
            {
                so = GameManager.Resource.Load<UnitDataSO>($"SO/Unit/AllData/UnitData_{_ID}");
                if (so == null) return;
            }

            InitSODataSetting(so);
            Initialized();
        }

        // 현재 SO 받으면 해당 정보를 넣어줌
        private void InitSODataSetting(UnitDataSO so)
        {
            _ID = so.Id;
            _unitName = so.Name;
            gameObject.name = _unitName;
            /*if (so.Status == null) return;
            _status = new ActionUnitStatus(so.Status);*/
            if (_status.Health > 0)
            {
                SetState(UnitStateType.Idle);
            }
        }
    }
}
