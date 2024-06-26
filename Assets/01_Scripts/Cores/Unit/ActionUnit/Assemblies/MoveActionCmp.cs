using Scripts.Enums;
using Scripts.Module;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cores.Unit.ActinoUnit.Assemblies
{
    [DisallowMultipleComponent]
    public class MoveActionCmp : ActionAssembly
    {
        private MoveModule _moveModule = new();

        [Header("= 이동 관련 =")]
        [SerializeField] private float _moveSpeed = 1.0f;
        [SerializeField] private int _movableCount = 0;

        private List<Vector2> _roadPos = new List<Vector2>();
        private Vector2 _goalRoadPos = Vector2.zero;
        private int _roadIndex = 0;
        private int _roadMaxIndex = 0;

        protected override void OnInitialized()
        {
            if (_moveModule == null)
                _moveModule = new MoveModule();

            _moveModule.Init(ActionUnitCore.Tr, ActionUnitCore.Tr.position, _moveSpeed);
            ActionAssemblyType = ActionType.Move;
            base.OnInitialized();
        }

        public bool Move()
        {
            _moveModule.Move();

            // 현재 이동 중이라면
            if (_moveModule.IsMoving)
                return true;

            // 다음 이동할 것이 있다면
            if (_roadIndex < _roadMaxIndex)
            {
                _roadIndex++;
                _moveModule.RunSetting(_roadPos[_roadIndex]);
                return true;
            }

            return false;
        }
        public bool WayToGo(List<Vector2> roadPos)
        {
            if (roadPos == null && roadPos.Count < 2)
            {
                Debug.Log("No RondPos");
                return false;
            }

            _roadPos = roadPos;
            _roadIndex = 1;
            _roadMaxIndex = roadPos.Count - 1;
            _moveModule.RunSetting(_roadPos[_roadIndex]);
            return true;
        }
        private void Test()
        {
            List<Vector2> road = new List<Vector2>();
            road.Add(new Vector2(Unit.Tr.position.x, Unit.Tr.position.y));
            road.Add(new Vector2(Unit.Tr.position.x + 1, Unit.Tr.position.y));
            road.Add(new Vector2(Unit.Tr.position.x + 2, Unit.Tr.position.y));

            WayToGo(road);
        }

        public override void KeyBoardAction(KeyboardInputType inputType)
        {
            if (this.State == AssemblyStateType.Ready)
            {
                bool isInput = false;
                Vector2 dir = Vector2.zero;

                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    isInput = true;
                    dir = Vector2.up;
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    isInput = true;
                    dir = Vector2.down;
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    isInput = true;
                    dir = Vector2.left;
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    isInput = true;
                    dir = Vector2.right;
                }

                if (isInput)
                {
                    _goalRoadPos = (Vector2)Unit.Tr.position + dir;
                    this.State = AssemblyStateType.Choose;
                }
            }
            else if (this.State == AssemblyStateType.Choose)
            {
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    // _roadPos = StageManager.Instance.GetRoad(Unit.Tr.position, _goalRoadPos);
                    WayToGo(_roadPos);
                    this.State = AssemblyStateType.Action;
                }
            }
        }
        public override void MouseAction(MouseInputType inputType, Vector3 mousePos)
        {
            if (this.State == AssemblyStateType.Ready)
            {
                base.MouseAction(inputType, mousePos);
                if (inputType != MouseInputType.Clicked)
                    return;

                // 클릭 되었을 때, 해당 좌표와 가장 가까운 노드의 좌표로 변환하고 길찾기
                _goalRoadPos = mousePos;
                this.State = AssemblyStateType.Choose;
            }
        }

    }
}