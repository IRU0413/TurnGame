using Scripts.Enums;
using Scripts.Interface;
using Scripts.Util;
using UnityEngine;
namespace Scripts.Cores.Unit.ActinoUnit
{
    public class ActionAssembly : UnitAssembly, IInputAble
    {
        public ActionUnitCore ActionUnitCore => Base as ActionUnitCore;

        private ActionType _actionType;
        [Header("= 행동 부품 상태 =")]
        [SerializeField] private AssemblyStateType _state = AssemblyStateType.None;

        [SerializeField] protected bool _isInputAbleMouse = false; // 마우스 입력 가능 여부
        [SerializeField] protected bool _isInputAbleKeyBoard = false; // 키보드 입력 기능 여부 

        public bool TakeAction
        {
            set
            {
                if (value)
                    State = AssemblyStateType.Ready;
                else
                    State = AssemblyStateType.None;
            }
        }
        public ActionType ActionAssemblyType
        {
            get
            {
                return _actionType;
            }
            protected set
            {
                _actionType = value;
            }
        }
        public AssemblyStateType State
        {
            get { return _state; }
            protected set
            {
                SetStateSetting(value);
            }
        }

        protected override void OnInitialized()
        {
            State = AssemblyStateType.None;
            /*if (ActionUnitCore.UnitType == UnitType.Player)
            {
                _isInputAbleKeyBoard = true;
                _isInputAbleMouse = true;
            }*/

            base.OnInitialized();
        }
        #region Input
        // Not Use Base
        public virtual void KeyBoardAction(KeyboardInputType inputType)
        {
            // this.LogPrint($"키보드 입력 받는중 => {inputType}");
        }
        // Not Use Base
        public virtual void MouseAction(MouseInputType inputType, Vector3 mousePos)
        {
            // this.LogPrint($"마우스 입력 받는중 => {inputType}, {mousePos}");
        }

        #endregion

        #region State Setting
        private void SetStateSetting(AssemblyStateType state)
        {
            if (!_isInitialized) return;
            // 다음 단계의 상태로 변환하거나 전 단계로 돌리는 것은가능. 한번에 2단계 변경은 안됨. 
            if ((int)_state + 1 < (int)state) return;

            switch (state)
            {
                case AssemblyStateType.None:
                    this.LogPrint($"초기화된 상태");
                    NoneStateSetting();
                    break;
                case AssemblyStateType.Ready:
                    this.LogPrint($"키 세팅 완료 및 명령 대기 상태");
                    ReadyStateSetting();
                    break;
                case AssemblyStateType.Choose:
                    this.LogPrint($"선택된 명령의 세부 명령 대기 상태");
                    ChooseStateSetting();
                    break;
                case AssemblyStateType.Action:
                    this.LogPrint($"입력을 받은 명령 실행 중 상태");
                    ActionStateSetting();
                    break;
                case AssemblyStateType.End:
                    this.LogPrint($"입력 실행을 받고 종료된 상태");
                    break;
                default:
                    break;
            }

            _state = state;
        }
        protected virtual void NoneStateSetting()
        {
            if (_isInputAbleKeyBoard)
                GameManager.Input.KeyAction -= KeyBoardAction;
            if (_isInputAbleMouse)
                GameManager.Input.MouseAction -= MouseAction;
        }
        protected virtual void ReadyStateSetting()
        {
            if (_isInputAbleKeyBoard)
            {
                this.LogPrint("키보드 입력 추가");
                GameManager.Input.KeyAction -= KeyBoardAction;
                GameManager.Input.KeyAction += KeyBoardAction;
            }
            if (_isInputAbleMouse)
            {
                this.LogPrint("마우스 입력 추가");
                GameManager.Input.MouseAction -= MouseAction;
                GameManager.Input.MouseAction += MouseAction;
            }
        }
        protected virtual void ChooseStateSetting() { }
        protected virtual void ActionStateSetting() { }
        protected virtual void EndStateSetting()
        {
            if (_isInputAbleKeyBoard)
                GameManager.Input.KeyAction -= KeyBoardAction;
            if (_isInputAbleMouse)
                GameManager.Input.MouseAction -= MouseAction;
        }

        #endregion 

        #region State Update
        private void StateUpdate()
        {
            if (!_isInitialized) return;

            switch (_state)
            {
                case AssemblyStateType.None:
                    NoneUpdate();
                    break;
                case AssemblyStateType.Ready:
                    ReadyUpdate();
                    break;
                case AssemblyStateType.Choose:
                    ChooseUpdate();
                    break;
                case AssemblyStateType.Action:
                    ActionUpdate();
                    break;
            }
        }
        protected virtual void NoneUpdate()
        {
            // this.LogPrint("NoneUpdate Fuction Update");
        }
        protected virtual void ReadyUpdate()
        {

        }
        protected virtual void ChooseUpdate()
        {
            // this.LogPrint("ChooseUpdate Fuction Update");
        }
        protected virtual void ActionUpdate()
        {
            // this.LogPrint("StateUpdate Fuction Update");
        }

        #endregion

        private void Update()
        {
            StateUpdate();
        }
    }
}
