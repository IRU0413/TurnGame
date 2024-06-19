using Scripts.Enums;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cores.Skills.ReSkillMain
{
    public class SkillActionCore : Core
    {
        private SkillMainCore _root = null;

        // ����
        [SerializeField] private SearchFormatType _searchFormat = SearchFormatType.None; // � ��������
        [SerializeField] private int _searchDistance = 1; // ������
        private List<Vector2> _baseRangeList = new();

        public SkillMainCore Root
        {
            get => _root;
            set => Setting(value);
        }
        public List<Vector2> RangeList => _baseRangeList;
        private void Setting(SkillMainCore root)
        {
            _baseRangeList = Algorithms.GetRangePos(_searchFormat, _searchDistance);
            _root = root;
            base.Initialized();
            base.Tr.parent = _root.Pocket.transform;
        }
    }
}