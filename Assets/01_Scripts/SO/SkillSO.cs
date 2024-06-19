namespace Scripts.SO
{
    using Scripts.Enums;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SkillSO", menuName = "ScriptableObjects/Skill")]
    public class SkillSO : ScriptableObject
    {
        #region Property
        [Header("���� ��ȣ")]
        [SerializeField][HideInInspector] private int _id;

        [Header("��ų �̸�")]
        [SerializeField] private string _name;

        [Header("��ų �ɷ� Ÿ��")]
        [SerializeField] private AbilityType _abilityType;

        [Header("��ų �ߵ� ����")]
        [SerializeField] private int _mana;
        [SerializeField] private int _coolTime;

        [Header("��ų ���� ����")]
        [SerializeField] private SearchFormatType _searchFormat;
        [SerializeField] private ActionPointCorrection _drawPoint;
        [SerializeField] private int _searchDistance;

        [SerializeField] private Sprite _Icon; // ������
        [SerializeField] private AudioClip _activeSound; // ����

        #endregion

        #region GetSet
        public int ID => _id;

        public AbilityType AbilityType => _abilityType;

        public string Name => _name;

        public int Mana => _mana;
        public int CoolTime => _coolTime;

        public SearchFormatType SearchFormat => _searchFormat;
        public ActionPointCorrection DrawPoint => _drawPoint;
        public int SearchDistance => _searchDistance;

        public Sprite Icon => _Icon;
        // public DictionaryNode<int, GameObject> SkillGo => _skillGo;
        public AudioClip ActiveSound => _activeSound;

        #endregion


        public void SaveData(
            int id,
            AbilityType abilityType,
            string name,
            int mana, int coolTime,
            SearchFormatType searchFormat, ActionPointCorrection DrawPoint, int searchDistance,
            Sprite icon, AudioClip skillActiveSound)
        {
            _id = id;

            _abilityType = abilityType;

            _name = name;

            _mana = mana;
            _coolTime = coolTime;

            _searchFormat = searchFormat;
            _drawPoint = DrawPoint;
            _searchDistance = searchDistance;

            _Icon = icon;
            // _skillGo = skillGameObject;
            _activeSound = skillActiveSound;
        }
    }
}