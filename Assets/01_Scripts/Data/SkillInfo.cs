using Scripts.Enums;
using Scripts.SO;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Data
{
    public class SkillInfo
    {
        private int _id;

        // 능력 타입
        // private AbilityType _abilityType;

        // 스킬 이름
        private string _name;

        // 필요 조건
        private float _mana;
        private int _coolTime;

        // 탐색 정보
        private SearchFormatType _searchFormat;
        private int _searchDistance;

        private List<Vector2> _searchNodesPos; // 탐색 범위 좌표

        // 효과 적용 유닛 최소 최대
        private int _minEffectApplyUnitCount;
        private int _maxEffectApplyUnitCount;

        // 리소스
        private Sprite _icon;
        private GameObject _skillGo;
        private AudioClip _activeSound;

        public int ID => _id;
        // public AbilityType AbilityType => _abilityType;

        public string Name => _name;

        public float Mana => _mana;
        public int CoolTime => _coolTime;

        public SearchFormatType SearchFormat => _searchFormat;
        public int SearchDistance => _searchDistance;
        public List<Vector2> SearchNodesPos => _searchNodesPos;

        public int MinCount => _minEffectApplyUnitCount;
        public int MaxCount => _maxEffectApplyUnitCount;

        public Sprite Icon => _icon;
        public GameObject SkillGo => _skillGo;
        public AudioClip SkillActiveSound => _activeSound;

        // public string GoName => $"Skill_{AbilityType}_{ItemName}_{ID}";

        public SkillInfo(int id, SkillSO so)
        {
            _id = id;

            // _abilityType = _stageSo.AbilityType;

            _name = so.Name;

            _mana = so.Mana;
            _coolTime = so.CoolTime;

            _searchFormat = so.SearchFormat;
            _searchDistance = so.SearchDistance;
            _searchNodesPos = Algorithms.GetRangePos(_searchFormat, _searchDistance);

            /*_minEffectApplyUnitCount = _stageSo.MinEffectApplyUnitCount;
            _maxEffectApplyUnitCount = _stageSo.MaxEffectApplyUnitCount;*/

            _icon = so.Icon;
            // _skillGo = _stageSo.SkillGo;
            _activeSound = so.ActiveSound;
        }

        public SkillInfo(AbilityType abilityType, string skillName, float mana, int coolTime, int min, int max, SearchFormatType searchFormat, int searchDistance, Sprite icon, GameObject skillGo, AudioClip activeSound)
        {
            // _abilityType = abilityType;

            _name = skillName;

            _mana = mana;
            _coolTime = coolTime;

            _searchFormat = searchFormat;
            _searchDistance = searchDistance;
            _searchNodesPos = Algorithms.GetRangePos(_searchFormat, _searchDistance);

            _minEffectApplyUnitCount = min;
            _maxEffectApplyUnitCount = max;

            _icon = icon;
            _skillGo = skillGo;
            _activeSound = activeSound;
        }

        public SkillInfo()
        {
            _name = "None";

            _mana = 1;
            _coolTime = 1;

            _minEffectApplyUnitCount = 1;
            _maxEffectApplyUnitCount = 2;

            _searchFormat = SearchFormatType.Circle;
            _searchDistance = 3;

            _searchNodesPos = Algorithms.GetRangePos(_searchFormat, _searchDistance);

            _skillGo = GameManager.Resource.Load<GameObject>($"Prefab/Skill/Test");
        }
    }
}
