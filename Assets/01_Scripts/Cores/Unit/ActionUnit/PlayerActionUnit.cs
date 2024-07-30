using Scripts.Enums;
using Scripts.Interface;
using Scripts.Manager;
using UnityEngine;

namespace Scripts.Cores.Unit.ActinoUnit
{
    public class PlayerActionUnit : ActionUnitCore, IInputAble
    {
        protected override void Initialized()
        {
            base.Initialized();
            GameManager.Input.KeyAction += KeyBoardAction;
            GameManager.Input.MouseAction += MouseAction;
        }

        public void MouseAction(MouseInputType inputType, Vector3 mousePos)
        {
            if (inputType == MouseInputType.None) return;
            // 플레이어의 마우스 입력 처리 로직 추가
        }

        public void KeyBoardAction(KeyboardInputType inputType)
        {
            if (_actionType == ActionType.None)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) Action = ActionType.Move;
                else if (Input.GetKeyDown(KeyCode.Alpha2)) Action = ActionType.Ability;
                else if (Input.GetKeyDown(KeyCode.C)) _isMyTurn = false;
            }
            else if (_actionAssemblies[_actionType].State != AssemblyStateType.Action &&
                     _actionAssemblies[_actionType].State != AssemblyStateType.End)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    Action = ActionType.None;
                }
            }
        }
    }
}
