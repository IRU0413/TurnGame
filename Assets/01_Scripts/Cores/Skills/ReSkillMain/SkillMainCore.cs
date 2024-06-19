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

        [SerializeField] private AbilityType _abilityType; // �ɷ� Ÿ��
        [SerializeField] private SkillState _state; // ����

        // �ʿ� �ڽ�Ʈ
        private int _mana;
        private int _coolTime;
        private int _maxCoolTime;

        // ����
        private SearchFormatType _searchFormat = SearchFormatType.None; // � ��������
        private int _searchDistance = 1; // ������
        private List<Vector2> _baseRangeList = new();
        private List<Vector2> _allActionRangeList = new();
        private Vector2 _actionPoint = Vector2.zero;


        // ���� ����
        [SerializeField] private List<SkillActionCore> _actionSkillList = new();

        // ������
        private ActionUnitCore _owner; // ������
        private AbilityActionCmp _ownerAbilityCmp; // ������ �ɷ� ��Ʈ�ѷ�

        // ���־�
        private GameObject _visualGroupGo = null; // ���־� ���� ����

        // ����
        private GameObject _skillObjectPocket = null; // ���� ����


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
            if (_isInitialized) return; // ���� �Ǿ��ٸ� ���� ���ϰ� ���´�.
            if (so == null) // �ּ� �ʿ� ���� SO�� �ִ���
            {
                so = SkillManager.Instance.GetSkillSO(_id);
                if (so == null)
                    return;
            }

            _abilityType = so.AbilityType;

            // �ʿ� ���Ʈ
            _mana = so.Mana;
            _maxCoolTime = so.CoolTime;
            _coolTime = _maxCoolTime;

            // Ž��
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

            _visualGroupGo = visualGo;
            this.LogPrint("���־� �ʱ�ȭ �Ϸ�");
            return true;
        }
        private void InitSkillObjPocket()
        {
            GameObject pocket = Tr.Find("Skill_Pocket")?.gameObject;
            if (pocket == null)
                pocket = Tr.CreateChildGameObject("Skill_Pocket");
            _skillObjectPocket = pocket;

            SkillActionCore[] findActionObjList = Tr.GetComponentsInChildren<SkillActionCore>();

            // ã�� ��ų ���� list
            foreach (var skillObj in findActionObjList)
            {
                if (_actionSkillList.Contains(skillObj))
                    continue;
                _actionSkillList.Add(skillObj);
            }

            // ���� ��ų ���� list
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
            this.LogPrint("���� �ʱ�ȭ �Ϸ�");
        }

        private void SetOwner(ActionUnitCore owner)
        {
            if (!_isInitialized) return;

            if (owner == null)
            {
                // ������ ���� ������ �ִٸ�
                if (_owner != null)
                {
                    _ownerAbilityCmp.RemoveAbility(this);
                }

                _owner = null;
                _ownerAbilityCmp = null;

                // ���־� ON
                _visualGroupGo.SetActive(false);
            }
            else
            {
                // �ش� ��ų�� ������ �ֳ�?
                // � ��û�� �׼� ������ �ʱ�ȭ�� �Ǿ� �ʾҴ°�?
                if (_owner != null || !owner.IsInitialized) return;

                // ���ο� �����ڿ� �ʿ� ������Ʈ�� �ִ���
                var newOwnerCmp = owner.GetAssembly<AbilityActionCmp>();
                if (newOwnerCmp == null) return;

                // ���־� OFF
                _visualGroupGo.SetActive(true);

                _owner = owner;
                _ownerAbilityCmp = newOwnerCmp;
                _ownerAbilityCmp.AddAbility(this);
            }
        }
        private void SetState(SkillState state)
        {
            // �ʱ�ȭ �ȵǾ��ų�
            // �ش� ��ų ������ ���ٸ�
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
            // ������� �ʴ� ���� > ���� ���� �Ǵ� ���� �Ϸ� �ǹ�
            /*foreach (var skillObj in _actionSkillList)
                skillObj.State = SkillState.None;*/
        }
        private void ReadyStateSetting()
        {

        }
        private void ActionStateSetting()
        {
            // ���� �� �ʿ��� ���� ���� �� ����
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