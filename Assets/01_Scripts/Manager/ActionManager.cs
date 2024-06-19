using Scripts.Cores.Unit.ActinoUnit;
using Scripts.Enums;
using Scripts.Pattern;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Manager
{
    public class ActionManager : MonoBehaviourSingleton<ActionManager>
    {
        private List<ActionUnitCore> _actionUnitList = new List<ActionUnitCore>();
        [SerializeField] private ActionUnitCore _nowActionUnit = null;

        public ActionUnitCore NowActionUnit => _nowActionUnit;

        public void Add(ActionUnitCore actionUnit)
        {
            if (actionUnit == null) return;

            bool isInUnit = UnitManager.Instance.ContainsUnit(actionUnit);
            if (!isInUnit)
            {
                Debug.LogWarning($"[ActionUnit({actionUnit.name}) >> Not Registration Unit Name]");
                return;
            }

            _actionUnitList.Add(actionUnit);
            Debug.Log($"[ActionUnit({actionUnit.name}) >> Add Name]");
        }

        public void Remove(ActionUnitCore actionUnit)
        {
            if (actionUnit == null) return;
            if (!IsContains(actionUnit)) return;

            _actionUnitList.Remove(actionUnit);
            Debug.LogWarning($"[ActionUnit({actionUnit.name}) >> Remove Name]");
        }

        public bool IsContains(ActionUnitCore actionUnit)
        {
            if (actionUnit == null) return false;
            return _actionUnitList.Contains(actionUnit);
        }

        public void SortActionUnit()
        {
            _actionUnitList.Sort((a, b) => b.ActionPoint.CompareTo(a.ActionPoint));
        }

        public void NextAction()
        {
            if (_actionUnitList.Count == 0) return;

            if (_nowActionUnit != null)
                _nowActionUnit.EndTurn();

            SortActionUnit();

            ActionUnitCore aUnit = _actionUnitList[0];
            float deductionActionPoint = aUnit.ActionPoint;

            aUnit.MyTurn();
            _nowActionUnit = aUnit;

            for (int i = 1; i < _actionUnitList.Count; i++)
                _actionUnitList[i].ActionPoint -= deductionActionPoint;
        }

        private void Update()
        {
            // 초기화 안되었을 때
            if (!base.IsInitialized) return;
            // 현재 행동 유닛이 없을 떄
            if (_nowActionUnit == null) return;

            if (_nowActionUnit.IsMyTurn)
            {
                HandleActionTypeInput();


                return;
            }

            // 다음 유닛 가지고 오기
            NextAction();
        }
        // 현재 유닛의 턴일 때,
        // 입력 감지

        // 턴인 유닛이 행동이 없는 경우, 행동 선택 시 > 행동 전환 및 행동 기능 상태 초기화
        // 턴인 유닛이 행동이 없는 경우, 턴 넘기기 선택 시 > 턴 넘기기

        // 행동 중인 행동이 있는데 준비 단계일 꼉우 > 행동 취소 가능
        // 행동 중인 행동이 있는데 준비 단계가 아닐 경우 > 행동 취소 불가능

        // 행동 중인 행동이 있는데 끝 단계일 경우 > 행동 선택 단계로 전환.

        private void HandleActionTypeInput()
        {
            if (_nowActionUnit.Action != ActionType.None)
                return;

            if (Input.GetKeyUp(KeyCode.Alpha0))
                _nowActionUnit.Action = ActionType.Move;
            else if (Input.GetKeyUp(KeyCode.Alpha1))
                _nowActionUnit.Action = ActionType.Ability;
            else if (Input.GetKeyUp(KeyCode.C))
                _nowActionUnit.EndTurn();
        }

        private void HandleActionStateInput()
        {
        }
    }
}
