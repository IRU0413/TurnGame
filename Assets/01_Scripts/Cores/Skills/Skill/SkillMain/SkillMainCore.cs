using Scripts.Cores.Skills.SkillObject;
using Scripts.Cores.Unit.ActinoUnit;
using Scripts.Cores.Unit.ActinoUnit.Assemblies;
using Scripts.Enums;
using Scripts.Manager;
using Scripts.SO;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cores.Skills.Skill.SkillMain
{
    public class SkillMainCore : SkillCore
    {
        [Header("= BASE =")]
        [SerializeField] private int _skillID = -1; // 초반에 세팅 될, 아이티
        private ActionUnitCore _owner; // 소유자
        private AbilityActionCmp _ownerActionCmp;

        [Header("= 스킬 타입 =")]
        [SerializeField] private AbilityType _abilityType = AbilityType.None;

        [Header("= 스킬 발동 필요 요소 =")]
        [SerializeField] private int _needMana = 0; // 필요 마나
        [SerializeField] private int _coolTime = 0; // 현재 쿨타임
        [SerializeField] private int _maxCoolTime = 0; // 최대 쿨타임

        [Header("= 스킬 리소스 =")]
        [SerializeField] private Sprite _icon;

        [Header("= 스킬 오브젝트 리스트 =")]
        [SerializeField] private List<SkillObjectCore> _haveSkillObjList = new();
        private GameObject _skillObjectPocket;

        private bool _isDrawAble = false;

        public ActionUnitCore Owner
        {
            get { return _owner; }
            set
            {
                if (!IsInitialized)
                {
                    this.ErrorPrint("소유자 등록 진행 하려 하였지만, 초기화 안되어있습니다. >> [비활성화 진행됩니다.]");
                    if (this.gameObject.activeSelf)
                        this.gameObject.SetActive(false);
                    return;
                }

                // 소유자가 있는데 다른 값이 들어왔거나
                // 소유자도 없고 넣어주려는 값도 없는 경우
                if (!(_owner != null ^ value != null))
                {
                    this.WarningPrint($"소유자가 존재 하지 않거고 소유자로 지정할 대상이 없거나 소유자가 이미 존재하여 변경 불가능합니다.\n " +
                        $"[_owner != null ^ value != null] => {_owner != null ^ value != null} / owner {_owner != null} value {value != null}");
                    return;
                }

                // 소유자는 있는데 값이 없는 경우
                if (value == null)
                {
                    // 기존 소유자의 권한을 없엔다.
                    // _ownerActionCmp.RemoveAbility(this);
                    _owner = null;
                    VisualGroup.SetActive(true);
                    return;
                }

                _ownerActionCmp = value.GetAssembly<AbilityActionCmp>();
                if (!_ownerActionCmp)
                {
                    return;
                }

                // _ownerActionCmp.AddAbility(this);
                VisualGroup.SetActive(false);
                _owner = value;
                this.LogPrint($"스킬의 소유자가 등록되었습니다. > [소유자]:{_owner.gameObject.name}");
            }
        }
        public AbilityType AbilityType => _abilityType;

        public int NeedMana => _needMana;
        public int CoolTime => _coolTime;
        public int MaxCoolTime => _maxCoolTime;
        private bool IsSkillObjAction
        {
            set
            {
                foreach (var skillObj in _haveSkillObjList)
                {
                    if (skillObj.State == SkillState.Action)
                        return;
                }
            }
        }
        public GameObject Pocket => _skillObjectPocket;

        #region Init
        protected override void Initialized()
        {
            CoreType = DrawLayerType.Base;
            base.Initialized();
        }
        // Initialized 전 실행
        private void InitSkillActvieNeedDataBySO(SkillSO so)
        {
            if (so.Mana < 0)
            {
                _needMana = 0;
                this.ErrorPrint($"해당 스킬은 SO.Mana 를(을) 확인 해주세요.[SO File Name > {so.name}]");
            }
            else
                _needMana = so.Mana;

            if (so.CoolTime < 0)
            {
                _maxCoolTime = 0;
                this.ErrorPrint($"해당 스킬은 SO.CoolTime 를(을) 확인 해주세요.[SO File Name > {so.name}]");
            }
            else
                _maxCoolTime = so.CoolTime;

            _coolTime = _maxCoolTime;
            _isDrawAble = false;

            this.LogPrint("필요값 초기화 완료");
        }
        private void InitSkillRangeBySO(SkillSO so)
        {
            _searchFormat = so.SearchFormat;
            _drawPoint = so.DrawPoint;
            _searchDistance = so.SearchDistance;
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

            VisualGroup = visualGo;
            this.LogPrint("비주얼 초기화 완료");
            return true;
        }

        // Initialized 후 실행
        private void InitSkillObjPocket()
        {
            Transform tr = this.transform;
            GameObject pocket = tr.Find("Skill_Pocket")?.gameObject;
            if (pocket == null)
                pocket = tr.CreateChildGameObject("Skill_Pocket");
            _skillObjectPocket = pocket;

            SkillObjectCore[] findSkillObjList = tr.GetComponentsInChildren<SkillObjectCore>();

            // 찾은 스킬 옵젝 list
            foreach (var skillObj in findSkillObjList)
            {
                if (_haveSkillObjList.Contains(skillObj))
                    continue;
                _haveSkillObjList.Add(skillObj);
            }

            // 보유 스킬 옵젝 list
            foreach (var skillObj in _haveSkillObjList)
            {
                skillObj.Init(this);
            }
            this.LogPrint("포켓 초기화 완료");
        }

        /// <summary>
        /// 외부에서 생성하고 데이터 주입 & 내부 아이디 존재 시, 내부 아이디로 데이터 주입
        /// 외부 주입 우선
        /// </summary>
        /// <param name="so">데이터</param>
        public void Create(SkillSO so = null)
        {
            // SO Value Setting
            if (so == null)
            {
                so = SkillManager.Instance.GetSkillSO(_skillID);
                if (so == null)
                    return;
            }
            this.LogPrint("SkillMain 초기화 시작");
            // 능력 타입
            _abilityType = so.AbilityType;

            // 발동
            InitSkillActvieNeedDataBySO(so);
            // 범위
            InitSkillRangeBySO(so);
            // 비주얼
            bool isVisual = InitSkillMainCoreVisualizationGroupBySO(so);

            if (!isVisual)
                return;

            Initialized();
            // 스킬 옵젝 포켓
            InitSkillObjPocket();
            this.LogPrint("SkillMain 초기화 완료");
        }

        #endregion

        #region Draw
        private void DrawRange()
        {
            if (!_isDrawAble) return;

            // 자신 위치
            base.SkillDrawRange(DrawLayerType.Base, Owner.Tr.position);

            // 옵젝 트로우
            var drawPoint = SkillManager.Instance.GetTilePointToPosition(_ownerActionCmp.TilePoint);
            drawPoint = SkillManager.Instance.GetBindTileLocationWithinRange(PointRangeList, drawPoint);

            foreach (var skillObj in _haveSkillObjList)
                skillObj.DrawRange(drawPoint);
        }
        private void ClearDrawRange()
        {
            base.ClearSkillDrawRange(DrawLayerType.Base);
            foreach (var skillObj in _haveSkillObjList)
                skillObj.ClearDrawRange();
        }

        #endregion

        #region State Setting
        protected override void NoneSetting()
        {
            ClearDrawRange();
            _isDrawAble = false;
        }
        protected override void ReadySetting()
        {
            ClearDrawRange();
            _isDrawAble = true;
        }
        protected override void ActionSetting()
        {
            ClearDrawRange();
            _isDrawAble = false;
        }

        #endregion

        protected override void StateUpdate()
        {
            switch (base.State)
            {
                case SkillState.None:
                    break;
                case SkillState.Ready:
                    DrawRange();
                    break;
                case SkillState.Action:
                    // 하나라도 실행 중인 것이 있다면 냅두자
                    foreach (var skillObj in _haveSkillObjList)
                    {
                        if (skillObj.State == SkillState.Action)
                            return;
                    }

                    // 모두 실행 중이 아니라면 실행 안함으로 만들어줌
                    State = SkillState.None;
                    break;
            }
        }
    }
}