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
        [Header("= �ൿ ��ǰ ���� =")]
        [SerializeField] private AssemblyStateType _state = AssemblyStateType.None;

        [SerializeField] protected bool _isInputAbleMouse = false; // ���콺 �Է� ���� ����
        [SerializeField] protected bool _isInputAbleKeyBoard = false; // Ű���� �Է� ��� ���� 

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
            // this.LogPrint($"Ű���� �Է� �޴��� => {inputType}");
        }
        // Not Use Base
        public virtual void MouseAction(MouseInputType inputType, Vector3 mousePos)
        {
            // this.LogPrint($"���콺 �Է� �޴��� => {inputType}, {mousePos}");
        }

        #endregion

        #region State Setting
        private void SetStateSetting(AssemblyStateType state)
        {
            if (!_isInitialized) return;
            // ���� �ܰ��� ���·� ��ȯ�ϰų� �� �ܰ�� ������ ��������. �ѹ��� 2�ܰ� ������ �ȵ�. 
            if ((int)_state + 1 < (int)state) return;

            switch (state)
            {
                case AssemblyStateType.None:
                    this.LogPrint($"�ʱ�ȭ�� ����");
                    NoneStateSetting();
                    break;
                case AssemblyStateType.Ready:
                    this.LogPrint($"Ű ���� �Ϸ� �� ��� ��� ����");
                    ReadyStateSetting();
                    break;
                case AssemblyStateType.Choose:
                    this.LogPrint($"���õ� ����� ���� ��� ��� ����");
                    ChooseStateSetting();
                    break;
                case AssemblyStateType.Action:
                    this.LogPrint($"�Է��� ���� ��� ���� �� ����");
                    ActionStateSetting();
                    break;
                case AssemblyStateType.End:
                    this.LogPrint($"�Է� ������ �ް� ����� ����");
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
                this.LogPrint("Ű���� �Է� �߰�");
                GameManager.Input.KeyAction -= KeyBoardAction;
                GameManager.Input.KeyAction += KeyBoardAction;
            }
            if (_isInputAbleMouse)
            {
                this.LogPrint("���콺 �Է� �߰�");
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
