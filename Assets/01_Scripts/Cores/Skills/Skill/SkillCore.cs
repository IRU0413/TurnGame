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
        [Header("= 상태 =")]
        [SerializeField] private DrawLayerType _coreType = DrawLayerType.None;
        [SerializeField] private SkillState _state = SkillState.None;

        // 범위
        [Header("= 스킬 범위 필수 설정 =")]
        [SerializeField] protected SearchFormatType _searchFormat = SearchFormatType.None; // 어떤 모형인지
        [SerializeField] protected ActionPointCorrection _drawPoint = ActionPointCorrection.Center; // 중심점 위치
        [SerializeField] protected int _searchDistance = 1; // 반지름

        private List<Vector2> _baseRangeList = new List<Vector2>(); // 설정에 맞춰져서 뽑아진 범위
        private List<Vector2> _pointDrawRangeList = new List<Vector2>(); // 포인트를 기준으로 그려진 포인트(이거 어빌리티 컴포넌트에서 관리하자)
        private Vector2 _actionPos = Vector2.zero; // 범위가 그려진 위치
        [SerializeField] protected bool _isDrawRange = false; // 그려졌는지 여부

        // 비주얼
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

        // 범위
        public bool IsDrawRange => _isDrawRange;
        public List<Vector2> PointRangeList => _pointDrawRangeList;

        // 비주얼
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
            this.LogPrint("범위 초기화 완료");
        }
        protected void SkillDrawRange(DrawLayerType type, Vector2 drawPos)
        {
            if (_actionPos == drawPos && _isDrawRange != false)
                return;

            // 검증
            if (type == DrawLayerType.None)
                return;

            // 필요 데이터
            _actionPos = drawPos;
            _pointDrawRangeList = GetRangePosList(_baseRangeList, _actionPos);

            // 기능
            SkillManager.Instance.ClearRange(type);
            SkillManager.Instance.DrawRange(type, _pointDrawRangeList);
            _isDrawRange = true;
        }
        protected void ClearSkillDrawRange(DrawLayerType type)
        {
            if (!_isDrawRange)
                return;

            // 검증
            if (type == DrawLayerType.None)
                return;

            // 기능
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
        // State 로 값 주입
        private void StateSetting(SkillState state)
        {
            switch (state)
            {
                case SkillState.None:
                    // 끝났거나 가동을 하지 않는 상태
                    NoneSetting();
                    break;
                case SkillState.Ready:
                    // 세팅
                    ReadySetting();
                    break;
                case SkillState.Action:
                    // 출격
                    ActionSetting();
                    break;
            }
            _state = state;
        }
        protected virtual void NoneSetting() { }
        protected virtual void ReadySetting() { }
        protected virtual void ActionSetting() { }

        // About StateUpdate
        // OnAction 관련해서 상속 받아서 재정의
        protected virtual void StateUpdate() { }

        private void Update()
        {
            if (!_isInitialized)
                return;
            StateUpdate();
        }
    }
}