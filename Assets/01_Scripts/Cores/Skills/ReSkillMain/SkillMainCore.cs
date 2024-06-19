using Scripts.Cores.Unit.ActinoUnit;
using Scripts.Cores.Unit.ActinoUnit.Assemblies;
using Scripts.Enums;
using Scripts.Manager;
using Scripts.SO;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cores.Skills.ReSkillMain
{
    public class SkillMainCore : Core
    {
        [SerializeField] private int _id;

        [SerializeField] private AbilityType _abilityType; // 능력 타입
        [SerializeField] private SkillState _state; // 상태

        // 필요 코스트
        private int _mana;
        private int _coolTime;
        private int _maxCoolTime;

        // 범위
        private SearchFormatType _searchFormat = SearchFormatType.None; // 어떤 모형인지
        private int _searchDistance = 1; // 반지름
        private List<Vector2> _baseRangeList = new();
        private List<Vector2> _allActionRangeList = new();
        private Vector2 _actionPoint = Vector2.zero;


        // 실행 옵젝
        [SerializeField] private List<SkillActionCore> _actionSkillList = new();

        // 소유자
        private ActionUnitCore _owner; // 소유자
        private AbilityActionCmp _ownerAbilityCmp; // 소유자 능력 컨트롤러

        // 비주얼
        private GameObject _visualGroupGo = null; // 비주얼 관련 옵젝

        // 포켓
        private GameObject _skillObjectPocket = null; // 포켓 옵젝


        public AbilityType AbilityType => _abilityType;
        public ActionUnitCore Owner
        {
            get => _owner;
            set => SetOwner(value);
        }
        public SkillState State
        {
            get => _state;
            set => SetState(value);
        }
        public Vector2 ActionPoint => _actionPoint;
        public List<Vector2> RangeList => _baseRangeList;
        public List<Vector2> ActionRangeList => _allActionRangeList;
        public GameObject Pocket => _skillObjectPocket;


        public void Create(SkillSO so = null)
        {
            if (_isInitialized) return; // 생성 되었다면 생성 못하게 막는다.
            if (so == null) // 최소 필요 정보 SO가 있는지
            {
                so = SkillManager.Instance.GetSkillSO(_id);
                if (so == null)
                    return;
            }

            _abilityType = so.AbilityType;

            // 필요 콧드트
            _mana = so.Mana;
            _maxCoolTime = so.CoolTime;
            _coolTime = _maxCoolTime;

            // 탐색
            _searchFormat = so.SearchFormat;
            // _actionPointCorrection = _stageSo.DrawPoint;
            _searchDistance = so.SearchDistance;
            _baseRangeList = Algorithms.GetRangePos(_searchFormat, _searchDistance);

            InitSkillMainCoreVisualizationGroupBySO(so);
            base.Initialized();
            InitSkillObjPocket();
        }
        private bool InitSkillMainCoreVisualizationGroupBySO(SkillSO so)
        {
            GameObject visualGo = gameObject.transform.Find("Skill_VisualGroup")?.gameObject;
            if (visualGo == null)
            {
                visualGo = GameManager.Resource.Instantiate("Skill/Skill_VisualGroup");
                // 그래도 없다면 초기화 실패
                if (visualGo == null)
                    return false;
            }
            visualGo.transform.parent = this.transform;

            GameObject iconGo = visualGo.transform.Find("Skill_Icon").gameObject; ;
            GameObject shadowGo = visualGo.transform.Find("Skill_Shadow").gameObject;
            if (so.Icon != null)
            {
                iconGo.GetOrAddComponent<SpriteRenderer>().sprite = so.Icon;
            }

            iconGo.SetActive(true);
            shadowGo.SetActive(true);

            _visualGroupGo = visualGo;
            this.LogPrint("비주얼 초기화 완료");
            return true;
        }
        private void InitSkillObjPocket()
        {
            GameObject pocket = Tr.Find("Skill_Pocket")?.gameObject;
            if (pocket == null)
                pocket = Tr.CreateChildGameObject("Skill_Pocket");
            _skillObjectPocket = pocket;

            SkillActionCore[] findActionObjList = Tr.GetComponentsInChildren<SkillActionCore>();

            // 찾은 스킬 옵젝 list
            foreach (var skillObj in findActionObjList)
            {
                if (_actionSkillList.Contains(skillObj))
                    continue;
                _actionSkillList.Add(skillObj);
            }

            // 보유 스킬 옵젝 list
            foreach (var skillObj in _actionSkillList)
            {
                skillObj.Root = this;
                foreach (var pos in skillObj.RangeList)
                {
                    if (_allActionRangeList.Contains(pos))
                        continue;
                    _allActionRangeList.Add(pos);
                }
            }
            this.LogPrint("포켓 초기화 완료");
        }

        private void SetOwner(ActionUnitCore owner)
        {
            if (!_isInitialized) return;

            if (owner == null)
            {
                // 주인이 기존 주인이 있다면
                if (_owner != null)
                {
                    _ownerAbilityCmp.RemoveAbility(this);
                }

                _owner = null;
                _ownerAbilityCmp = null;

                // 비주얼 ON
                _visualGroupGo.SetActive(false);
            }
            else
            {
                // 해당 스킬의 주인이 있나?
                // 등독 요청한 액션 유닛이 초기화가 되어 않았는가?
                if (_owner != null || !owner.IsInitialized) return;

                // 새로운 소유자에 필요 컴포넌트가 있는지
                var newOwnerCmp = owner.GetAssembly<AbilityActionCmp>();
                if (newOwnerCmp == null) return;

                // 비주얼 OFF
                _visualGroupGo.SetActive(true);

                _owner = owner;
                _ownerAbilityCmp = newOwnerCmp;
                _ownerAbilityCmp.AddAbility(this);
            }
        }
        private void SetState(SkillState state)
        {
            // 초기화 안되었거나
            // 해당 스킬 소유자 없다면
            if (!_isInitialized || _owner == null) return;

            switch (state)
            {
                case SkillState.None:
                    NoneStateSetting();
                    break;
                case SkillState.Ready:
                    ReadyStateSetting();
                    break;
                case SkillState.Action:
                    ActionStateSetting();
                    break;
            }

            _state = state;
        }
        private void NoneStateSetting()
        {
            // 사용하지 않는 상태 > 실행 안함 또는 실행 완료 의미
            /*foreach (var skillObj in _actionSkillList)
                skillObj.State = SkillState.None;*/
        }
        private void ReadyStateSetting()
        {

        }
        private void ActionStateSetting()
        {
            // 실행 시 필요한 값들 감소 및 리셋
            _coolTime = _maxCoolTime;
            _owner.Status.Mana -= _mana;

            /* foreach (var skillObj in _actionSkillList)
                 skillObj.State = SkillState.OnAction;*/
        }

        private void StateUpdate()
        {
            switch (_state)
            {
                case SkillState.None:
                    break;
                case SkillState.Ready:
                    ReadyStateUpdate();
                    break;
                case SkillState.Action:
                    break;
            }
        }
        private void ReadyStateUpdate()
        {
            if (_actionPoint != _ownerAbilityCmp.TilePoint)
                _actionPoint = SkillManager.Instance.GetBindTileLocationWithinRange(_baseRangeList, _ownerAbilityCmp.TilePoint - (Vector2)Owner.Tr.position);


        }
        private void LateUpdate()
        {
            StateUpdate();
        }
    }
}