using Scripts.Enums;
using UnityEngine;

namespace Scripts.Cores.Unit.Assemblies
{
    public class PlayerAttackController : UnitController
    {
        protected override bool StartActionPoint()
        {
            return true;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                return true;
            return false;
        }
        protected override bool CancleActionPoint()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Unit.SetState(UnitStateType.Idle);
                return true;
            }
            return false;
        }

        protected override bool Action()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Unit.SetState(UnitStateType.Move);
                return true;
            }
            return false;
        }
    }
}