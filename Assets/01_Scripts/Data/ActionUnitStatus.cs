using Scripts.Interface;
using Scripts.SO;

namespace Scripts.Data
{
    public class ActionUnitStatus : IUnitStatus
    {
        private float _baseHealth;
        private float _baseMana;

        private float _baseAttackPower;
        private float _baseMagicPower;

        private float _baseArmor;
        private float _baseMagicResistance;

        private float _baseCriticalStrikeChance;
        private float _baseCriticalStrikeDamage;

        private float _baseArmorPenetration;
        private float _baseMagicPenetration;

        private float _baseActionSpeed;

        private int _baseMovePoint;


        private float _currentHealth;
        private float _healthMax;
        private float _currentMana;
        private float _manaMax;

        private float _currentAttackPower;
        private float _currentMagicPower;

        private float _currentArmor;
        private float _currentMagicResistance;

        private float _currentCriticalStraikeChance;
        private float _currentCriticalStraikeDamage;

        private float _currentArmorPenetration;
        private float _currentMagicPenetration;

        private float _currentActionSpeed;

        // base
        public float BaseHealth => _baseHealth;
        public float BaseMana => _baseMana;

        public float BaseAttackPower => _baseAttackPower;
        public float BaseMagicPower => _baseMagicPower;

        public float BaseArmor => _baseArmor;
        public float BaseMagicResistance => _baseMagicResistance;

        public float BaseCriticalStrikeChance => _baseCriticalStrikeChance;
        public float BaseCriticalStrikeDamage => _baseCriticalStrikeDamage;

        public float BaseArmorPenetration => _baseArmorPenetration;
        public float BaseMagicPenetration => _baseMagicPenetration;

        public float BaseActionSpeed => _baseActionSpeed;

        public int BaseMovePoint => _baseMovePoint;


        public float Health
        {
            get
            {
                return _currentHealth;
            }

            set
            {
                _currentHealth = value;

                if (_currentHealth > _healthMax)
                    _currentHealth = _healthMax;
            }
        }
        public float HealthMax
        {
            get
            {
                return _healthMax;
            }
            set
            {
                if (value <= 0)
                    return;

                float gap = value - _healthMax;

                _healthMax = value;

                if (0 < gap)
                {
                    Health += gap;
                }
                else
                {
                    if (_currentHealth > _healthMax)
                        Health += gap;
                }
            }
        }
        public float Mana
        {
            get
            {
                return _currentMana;
            }
            set
            {
                _currentMana = value;

                if (_currentMana > _manaMax)
                    _currentMana = _manaMax;
            }
        }
        public float ManaMax
        {
            get
            {
                return _manaMax;
            }
            set
            {
                _manaMax = value;

                float gap = value - _manaMax;

                if (0 < gap)
                {
                    Mana += gap;
                }
                else
                {
                    if (_currentMana > _manaMax)
                        Mana += gap;
                }
            }
        }

        public float CurrActionSpeed => _currentActionSpeed;

        public ActionUnitStatus(ActionUnitStatusSO so)
        {
            _baseHealth = so.Health;
            _baseMana = so.Mana;

            _baseAttackPower = so.AttackPower;
            _baseMagicPower = so.MagicPower;

            _baseArmor = so.Armor;
            _baseMagicResistance = so.MagicResistance;

            _baseCriticalStrikeChance = so.CriticalStrikeChance;
            _baseCriticalStrikeDamage = so.CriticalStrikeDamage;

            _baseArmorPenetration = so.ArmorPenetration;
            _baseMagicPenetration = so.MagicPenetration;

            _baseActionSpeed = so.ActionSpeed;

            _baseMovePoint = so.MovePoint;


            _healthMax = _baseHealth;
            _currentHealth = _healthMax;
            _manaMax = _baseMana;
            _currentMana = _manaMax;

            _currentAttackPower = _baseAttackPower;
            _currentMagicPower = _baseMagicPower;

            _currentArmor = _baseArmor;
            _currentMagicResistance = _baseMagicResistance;

            _currentCriticalStraikeChance = _baseCriticalStrikeChance;
            _currentCriticalStraikeDamage = _baseCriticalStrikeDamage;

            _currentArmorPenetration = _baseArmorPenetration;
            _currentMagicPenetration = _baseMagicPenetration;

            _currentActionSpeed = _baseActionSpeed;
        }
    }
}