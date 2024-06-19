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
            // �ʱ�ȭ �ȵǾ��� ��
            if (!base.IsInitialized) return;
            // ���� �ൿ ������ ���� ��
            if (_nowActionUnit == null) return;

            if (_nowActionUnit.IsMyTurn)
            {
                HandleActionTypeInput();


                return;
            }

            // ���� ���� ������ ����
            NextAction();
        }
        // ���� ������ ���� ��,
        // �Է� ����

        // ���� ������ �ൿ�� ���� ���, �ൿ ���� �� > �ൿ ��ȯ �� �ൿ ��� ���� �ʱ�ȭ
        // ���� ������ �ൿ�� ���� ���, �� �ѱ�� ���� �� > �� �ѱ��

        // �ൿ ���� �ൿ�� �ִµ� �غ� �ܰ��� �b�� > �ൿ ��� ����
        // �ൿ ���� �ൿ�� �ִµ� �غ� �ܰ谡 �ƴ� ��� > �ൿ ��� �Ұ���

        // �ൿ ���� �ൿ�� �ִµ� �� �ܰ��� ��� > �ൿ ���� �ܰ�� ��ȯ.

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
