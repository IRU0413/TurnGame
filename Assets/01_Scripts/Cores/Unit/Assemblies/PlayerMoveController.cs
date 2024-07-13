using Scripts.Enums;
using Scripts.Module;
using UnityEngine;

namespace Scripts.Cores.Unit.Assemblies
{
    // 모델 겸 컨트롤러 역활을 하고 있는듯
    public class PlayerMoveController : UnitController
    {
        [SerializeField] private float _moveSpeed = 3.0f;
        private Vector2 _moveVector = Vector2.zero;

        private MoveModule _moveModule = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _moveModule.Init(Unit.Tr, Unit.Tr.transform.position);
        }

        protected override bool CancleActionPoint()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Unit.SetState(UnitStateType.Idle);
                _moveModule.DirectionRunSetting(Vector2.zero);
                return true;
            }
            return false;
        }
        protected override bool StartActionPoint()
        {
            return true;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                return true;
            return false;
        }
        protected override bool Action()
        {
            _moveVector.x = Input.GetAxisRaw("Horizontal");
            _moveVector.y = Input.GetAxisRaw("Vertical");
            if (_moveVector.x != 0 || _moveVector.y != 0)
            {
                if (_moveVector.x == -1)
                    Unit.Ani.FlipY(false);
                else if (_moveVector.x == 1)
                    Unit.Ani.FlipY(true);

                Unit.SetState(UnitStateType.Move);

                _moveModule.DirectionRunSetting(_moveVector);
                _moveModule.Move(_moveSpeed);
                return false;
            }
            // 끝
            else
            {
                Unit.SetState(UnitStateType.Idle);
                _moveModule.DirectionRunSetting(Vector2.zero);
                return true;
            }
        }
    }
}