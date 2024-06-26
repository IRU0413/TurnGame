namespace Scripts.SO
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "UnitStatusSO", menuName = "ScriptableObjects/Unit/Status")]
    public class ActionUnitStatusSO : ScriptableObject
    {
        [SerializeField][HideInInspector] private int _id;

        [SerializeField] private float _health;
        [SerializeField] private float _mana;

        [SerializeField] private float _attackPower;
        [SerializeField] private float _magicPower;

        [SerializeField] private float _armor;
        [SerializeField] private float _magicResistance;

        [SerializeField] private float _criticalStrikeChance;
        [SerializeField] private float _criticalStrikeDamage;

        [SerializeField] private float _armorPenetration;
        [SerializeField] private float _magicPenetration;

        [SerializeField] private float _actionSpeed;

        [SerializeField] private int _movePoint;

        public int ID => _id;
        public float Health => _health;
        public float Mana => _mana;

        public float AttackPower => _attackPower;
        public float MagicPower => _magicPower;

        public float Armor => _armor;
        public float MagicResistance => _magicResistance;

        public float CriticalStrikeChance => _criticalStrikeChance;
        public float CriticalStrikeDamage => _criticalStrikeDamage;

        public float ArmorPenetration => _armorPenetration;
        public float MagicPenetration => _magicPenetration;

        public float ActionSpeed => _actionSpeed;

        public int MovePoint => _movePoint;

        public void SaveData(
            int id,
            float health, float mana,
            float attackPower, float magicPower,
            float armor, float magicResistance,
            float criticalStrikeChance, float criticalStrikeDamage,
            float armorPenetration, float magicPenetration,
            float activeSpeed,
            int movePoint
        )
        {
            _id = id;

            _health = health;
            _mana = mana;

            _attackPower = attackPower;
            _magicPower = magicPower;

            _armor = armor;
            _magicResistance = magicResistance;

            _criticalStrikeChance = criticalStrikeChance;
            _criticalStrikeDamage = criticalStrikeDamage;

            _armorPenetration = armorPenetration;
            _magicPenetration = magicPenetration;

            _actionSpeed = activeSpeed;

            _movePoint = movePoint;
        }
    }
}