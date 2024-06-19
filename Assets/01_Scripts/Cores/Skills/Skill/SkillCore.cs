using Scripts.Enums;
using Scripts.Interface;
using Scripts.Manager;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cores.Skills.Skill
{
    public abstract class SkillCore : Core, IVisualizationAble
    {
        #region Property
        [Header("= ���� =")]
        [SerializeField] private DrawLayerType _coreType = DrawLayerType.None;
        [SerializeField] private SkillState _state = SkillState.None;

        // ����
        [Header("= ��ų ���� �ʼ� ���� =")]
        [SerializeField] protected SearchFormatType _searchFormat = SearchFormatType.None; // � ��������
        [SerializeField] protected ActionPointCorrection _drawPoint = ActionPointCorrection.Center; // �߽��� ��ġ
        [SerializeField] protected int _searchDistance = 1; // ������

        private List<Vector2> _baseRangeList = new List<Vector2>(); // ������ �������� �̾��� ����
        private List<Vector2> _pointDrawRangeList = new List<Vector2>(); // ����Ʈ�� �������� �׷��� ����Ʈ(�̰� �����Ƽ ������Ʈ���� ��������)
        private Vector2 _actionPos = Vector2.zero; // ������ �׷��� ��ġ
        [SerializeField] protected bool _isDrawRange = false; // �׷������� ����

        // ���־�
        private GameObject _visualGroupGo = null;

        #endregion

        #region GetSet
        // BASE
        public DrawLayerType CoreType
        {
            get => _coreType;
            protected set
            {
                if (_isInitialized)
                    return;

                _coreType = value;
            }
        }
        public SkillState State
        {
            get => _state;
            set
            {
                if (SkillState.None == value || _state < value)
                {
                    StateSetting(value);
                }
            }
        }

        // ����
        public bool IsDrawRange => _isDrawRange;
        public List<Vector2> PointRangeList => _pointDrawRangeList;

        // ���־�
        public virtual GameObject VisualGroup
        {
            get => _visualGroupGo;
            set
            {
                if (_visualGroupGo != null)
                    return;
                _visualGroupGo = value;
            }
        }
        public virtual bool IsVisualable
        {
            get
            {
                if (_visualGroupGo == null)
                    return false;
                return _visualGroupGo.activeSelf;
            }
            set
            {
                if (_visualGroupGo == null)
                    return;
                _visualGroupGo.SetActive(value);
            }
        }

        #endregion

        protected override void Initialized()
        {
            SkillRangeSetting();
            base.Initialized();
        }

        // About DrawRange
        protected void SkillRangeSetting()
        {
            if (_searchFormat == SearchFormatType.None)
            {
                _searchFormat = SearchFormatType.Circle;
                _drawPoint = ActionPointCorrection.Center;
                _searchDistance = 0;
            }

            if (_baseRangeList == null)
                _baseRangeList = new();

            _baseRangeList = Algorithms.GetRangePos(_searchFormat, _searchDistance);
            // _baseRangeList = GetDrawPointRange(_baseRangeList, _actionPointCorrection);
            this.LogPrint("���� �ʱ�ȭ �Ϸ�");
        }
        protected void SkillDrawRange(DrawLayerType type, Vector2 drawPos)
        {
            if (_actionPos == drawPos && _isDrawRange != false)
                return;

            // ����
            if (type == DrawLayerType.None)
                return;

            // �ʿ� ������
            _actionPos = drawPos;
            _pointDrawRangeList = GetRangePosList(_baseRangeList, _actionPos);

            // ���
            SkillManager.Instance.ClearRange(type);
            SkillManager.Instance.DrawRange(type, _pointDrawRangeList);
            _isDrawRange = true;
        }
        protected void ClearSkillDrawRange(DrawLayerType type)
        {
            if (!_isDrawRange)
                return;

            // ����
            if (type == DrawLayerType.None)
                return;

            // ���
            SkillManager.Instance.ClearRange(type);
            _isDrawRange = false;
        }

        private List<Vector2> GetRangePosList(List<Vector2> posList, Vector2 drawPos)
        {
            List<Vector2> resultLIst = new();
            for (int i = 0; i < posList.Count; i++)
                resultLIst.Add(posList[i] + drawPos);

            return resultLIst;
        }
        private List<Vector2> GetDrawPointRange(List<Vector2> posList, ActionPointCorrection drawPoint)
        {
            List<Vector2> resultLIst = new();

            return resultLIst;
        }


        // About StateSetting
        // State �� �� ����
        private void StateSetting(SkillState state)
        {
            switch (state)
            {
                case SkillState.None:
                    // �����ų� ������ ���� �ʴ� ����
                    NoneSetting();
                    break;
                case SkillState.Ready:
                    // ����
                    ReadySetting();
                    break;
                case SkillState.Action:
                    // ���
                    ActionSetting();
                    break;
            }
            _state = state;
        }
        protected virtual void NoneSetting() { }
        protected virtual void ReadySetting() { }
        protected virtual void ActionSetting() { }

        // About StateUpdate
        // OnAction �����ؼ� ��� �޾Ƽ� ������
        protected virtual void StateUpdate() { }

        private void Update()
        {
            if (!_isInitialized)
                return;
            StateUpdate();
        }
    }
}