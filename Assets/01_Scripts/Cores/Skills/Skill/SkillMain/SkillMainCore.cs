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
        [SerializeField] private int _skillID = -1; // �ʹݿ� ���� ��, ����Ƽ
        private ActionUnitCore _owner; // ������
        private AbilityActionCmp _ownerActionCmp;

        [Header("= ��ų Ÿ�� =")]
        [SerializeField] private AbilityType _abilityType = AbilityType.None;

        [Header("= ��ų �ߵ� �ʿ� ��� =")]
        [SerializeField] private int _needMana = 0; // �ʿ� ����
        [SerializeField] private int _coolTime = 0; // ���� ��Ÿ��
        [SerializeField] private int _maxCoolTime = 0; // �ִ� ��Ÿ��

        [Header("= ��ų ���ҽ� =")]
        [SerializeField] private Sprite _icon;

        [Header("= ��ų ������Ʈ ����Ʈ =")]
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
                    this.ErrorPrint("������ ��� ���� �Ϸ� �Ͽ�����, �ʱ�ȭ �ȵǾ��ֽ��ϴ�. >> [��Ȱ��ȭ ����˴ϴ�.]");
                    if (this.gameObject.activeSelf)
                        this.gameObject.SetActive(false);
                    return;
                }

                // �����ڰ� �ִµ� �ٸ� ���� ���԰ų�
                // �����ڵ� ���� �־��ַ��� ���� ���� ���
                if (!(_owner != null ^ value != null))
                {
                    this.WarningPrint($"�����ڰ� ���� ���� �ʰŰ� �����ڷ� ������ ����� ���ų� �����ڰ� �̹� �����Ͽ� ���� �Ұ����մϴ�.\n " +
                        $"[_owner != null ^ value != null] => {_owner != null ^ value != null} / owner {_owner != null} value {value != null}");
                    return;
                }

                // �����ڴ� �ִµ� ���� ���� ���
                if (value == null)
                {
                    // ���� �������� ������ ������.
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
                this.LogPrint($"��ų�� �����ڰ� ��ϵǾ����ϴ�. > [������]:{_owner.gameObject.name}");
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
        // Initialized �� ����
        private void InitSkillActvieNeedDataBySO(SkillSO so)
        {
            if (so.Mana < 0)
            {
                _needMana = 0;
                this.ErrorPrint($"�ش� ��ų�� SO.Mana ��(��) Ȯ�� ���ּ���.[SO File Name > {so.name}]");
            }
            else
                _needMana = so.Mana;

            if (so.CoolTime < 0)
            {
                _maxCoolTime = 0;
                this.ErrorPrint($"�ش� ��ų�� SO.CoolTime ��(��) Ȯ�� ���ּ���.[SO File Name > {so.name}]");
            }
            else
                _maxCoolTime = so.CoolTime;

            _coolTime = _maxCoolTime;
            _isDrawAble = false;

            this.LogPrint("�ʿ䰪 �ʱ�ȭ �Ϸ�");
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
                // �׷��� ���ٸ� �ʱ�ȭ ����
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
            this.LogPrint("���־� �ʱ�ȭ �Ϸ�");
            return true;
        }

        // Initialized �� ����
        private void InitSkillObjPocket()
        {
            Transform tr = this.transform;
            GameObject pocket = tr.Find("Skill_Pocket")?.gameObject;
            if (pocket == null)
                pocket = tr.CreateChildGameObject("Skill_Pocket");
            _skillObjectPocket = pocket;

            SkillObjectCore[] findSkillObjList = tr.GetComponentsInChildren<SkillObjectCore>();

            // ã�� ��ų ���� list
            foreach (var skillObj in findSkillObjList)
            {
                if (_haveSkillObjList.Contains(skillObj))
                    continue;
                _haveSkillObjList.Add(skillObj);
            }

            // ���� ��ų ���� list
            foreach (var skillObj in _haveSkillObjList)
            {
                skillObj.Init(this);
            }
            this.LogPrint("���� �ʱ�ȭ �Ϸ�");
        }

        /// <summary>
        /// �ܺο��� �����ϰ� ������ ���� & ���� ���̵� ���� ��, ���� ���̵�� ������ ����
        /// �ܺ� ���� �켱
        /// </summary>
        /// <param name="so">������</param>
        public void Create(SkillSO so = null)
        {
            // SO Value Setting
            if (so == null)
            {
                so = SkillManager.Instance.GetSkillSO(_skillID);
                if (so == null)
                    return;
            }
            this.LogPrint("SkillMain �ʱ�ȭ ����");
            // �ɷ� Ÿ��
            _abilityType = so.AbilityType;

            // �ߵ�
            InitSkillActvieNeedDataBySO(so);
            // ����
            InitSkillRangeBySO(so);
            // ���־�
            bool isVisual = InitSkillMainCoreVisualizationGroupBySO(so);

            if (!isVisual)
                return;

            Initialized();
            // ��ų ���� ����
            InitSkillObjPocket();
            this.LogPrint("SkillMain �ʱ�ȭ �Ϸ�");
        }

        #endregion

        #region Draw
        private void DrawRange()
        {
            if (!_isDrawAble) return;

            // �ڽ� ��ġ
            base.SkillDrawRange(DrawLayerType.Base, Owner.Tr.position);

            // ���� Ʈ�ο�
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
                    // �ϳ��� ���� ���� ���� �ִٸ� ������
                    foreach (var skillObj in _haveSkillObjList)
                    {
                        if (skillObj.State == SkillState.Action)
                            return;
                    }

                    // ��� ���� ���� �ƴ϶�� ���� �������� �������
                    State = SkillState.None;
                    break;
            }
        }
    }
}