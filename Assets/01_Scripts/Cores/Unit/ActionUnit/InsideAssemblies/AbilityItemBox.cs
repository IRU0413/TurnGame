using Scripts.Cores.Skills.ReSkillMain;
using Scripts.Enums;
using Scripts.Util;
using UnityEngine;

namespace Scripts.Cores.Unit.ActinoUnit.InsideAssemblies
{
    public class AbilityItemBox
    {
        private bool _isInitialized = false;

        private SkillMainCore[] _weaponAbility;
        private SkillMainCore[] _passiveSkill;
        private SkillMainCore[] _activeSkill;

        // 스킬들을 담는 그룹
        private GameObject _skillGroupGo;

        public bool IsInitialized => _isInitialized;
        public GameObject SkillGroupGo => _skillGroupGo;

        #region Init
        public void Init(Transform parentTr, int weaponSlot = 1, int passiveSlot = 1, int activeSlot = 2)
        {
            InitSkillSlot(weaponSlot, passiveSlot, activeSlot);
            InitSkillGroup(parentTr);

            _isInitialized = true;
        }
        private void InitSkillSlot(int weaponSlot = 1, int passiveSlot = 1, int activeSlot = 2)
        {
            _weaponAbility = new SkillMainCore[weaponSlot];
            _passiveSkill = new SkillMainCore[passiveSlot];
            _activeSkill = new SkillMainCore[activeSlot];
        }
        private void InitSkillGroup(Transform parentTr)
        {
            _skillGroupGo = parentTr.CreateChildGameObject("SkillGroup");
        }

        #endregion

        #region A.R.S
        // 추가를 시전함, 비어있는 칸 찾아봄=> 없으면 취소, 있으면 강제로 바꿀것인가 여부에 따라서 교환
        public void Add(SkillMainCore skill)
        {
            if (!InitDebug()) return;
            if (skill == null) return;

            SkillMainCore[] useArray = null;

            useArray = GetAbilityTypeArray(skill.AbilityType); // 배열
            if (useArray == null) return;

            int addAbleIdx = -1;
            bool isAddAble = IsAddAble(useArray, out addAbleIdx);
            // 추가 할 수 없는 상황
            if (!isAddAble)
                return;

            useArray[addAbleIdx] = skill; // 추가
            // Debug.Log($"skill.Tr.parent => {skill.Tr.parent}");
            skill.Tr.parent = _skillGroupGo.transform;
        }
        public void Remove(AbilityType type, int idx = 0)
        {
            if (!InitDebug()) return;

            SkillMainCore[] useArray = null;

            useArray = GetAbilityTypeArray(type); // 배열
            if (useArray == null) return;
            if (idx < 0 || idx <= useArray.Length)
                return;

            SkillMainCore removeSkill = useArray[idx];

            removeSkill.Tr.parent = null;
            useArray[idx] = null; // 제거
        }
        public void Remove(SkillMainCore skill)
        {
            if (!InitDebug()) return;
            if (skill == null) return;

            SkillMainCore[] useArray = null;
            int useIdx = -1;

            useArray = GetAbilityTypeArray(skill.AbilityType); // 배열
            if (useArray == null) return;

            useIdx = GetContainsAbilityIndex(useArray, skill); // 인텍스
            if (useIdx < 0) return;

            useArray[useIdx] = null; // 제거
        }
        public SkillMainCore Select(AbilityType type, int idx = 0)
        {
            // 초기화 여부
            if (!InitDebug())
                return null;

            SkillMainCore[] useArray = null;
            SkillMainCore selectedSkillMainCore = null;
            int useIdx = -1;

            useArray = GetAbilityTypeArray(type);
            if (useArray == null) return null;

            useIdx = GetCorrectionIndex(useArray, idx);

            selectedSkillMainCore = useArray[useIdx];
            return selectedSkillMainCore;
        }

        #endregion

        #region Util
        private bool InitDebug()
        {
            if (!_isInitialized)
            {
                Debug.Log($"{this.GetType().Name} 되지 않았습니다.");
            }
            return _isInitialized;
        }
        private SkillMainCore[] GetAbilityTypeArray(AbilityType type)
        {
            SkillMainCore[] selectedArr = null;
            switch (type)
            {
                case AbilityType.Weapon:
                    selectedArr = _weaponAbility;
                    break;
                case AbilityType.Passive:
                    selectedArr = _passiveSkill;
                    break;
                case AbilityType.Action:
                    selectedArr = _activeSkill;
                    break;
                default:
                    Debug.Log("찾고자하는 배열의 타입이 None입니다.");
                    break;
            }

            if (selectedArr == null)
                Debug.LogError($"[{_skillGroupGo.transform.parent}][{GetType().Name}]의 지정타입({type}) 배열을 찾을 수 없습니다.");

            return selectedArr;
        }
        private int GetCorrectionIndex(SkillMainCore[] skills, int idx)
        {
            if (skills == null)
                idx = -1;
            else
            {
                if (skills.Length <= idx)
                    idx = _weaponAbility.Length - 1;
            }

            return idx;
        }
        private int GetContainsAbilityIndex(SkillMainCore[] skills, SkillMainCore skill)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                if (skills[i].Equals(skill))
                    return i;
            }

            return -1;
        }
        public bool IsAddAble(AbilityType type)
        {
            var typeArray = GetAbilityTypeArray(type);
            bool isAddAble = IsAddAble(typeArray, out _);
            return isAddAble;
        }
        private bool IsAddAble(SkillMainCore[] typeArray, out int idx)
        {
            for (int i = 0; i < typeArray.Length; i++)
            {
                if (typeArray[i] == null)
                {
                    idx = i;
                    return true;
                }
            }
            idx = -1;
            return false;
        }

        #endregion
    }
}
