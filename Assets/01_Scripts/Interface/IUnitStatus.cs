namespace Scripts.Interface
{
    public interface IUnitStatus
    {
        float BaseHealth { get; } // 체력
        float BaseMana { get; } // 마나

        float BaseAttackPower { get; } // 공격력
        float BaseMagicPower { get; } // 주문력

        float BaseArmor { get; } // 방어력
        float BaseMagicResistance { get; } // 마법 저항력

        float BaseCriticalStrikeChance { get; } // 치명타 확률
        float BaseCriticalStrikeDamage { get; } // 치명타 데미지

        float BaseArmorPenetration { get; } // 방어 관통력
        float BaseMagicPenetration { get; } // 마법 관통력

        float BaseActionSpeed { get; } // 행동 스피드
        int BaseMovePoint { get; } // 행동 스피드



        /*float Health { get; set; } // 체력
        float HealthMax { get; set; } // 체력
        float Mana { get; set; } // 마나
        float ManaMax { get; set; } // 마나

        float CurrAttackPower { get; set; } // 공격력
        float CurrMagicPower { get; set; } // 주문력

        float CurrArmor { get; set; } // 방어력
        float CurrMagicResistance { get; set; } // 마법 저항력

        float CurrCriticalStrikeChance { get; set; } // 치명타 확률
        float CurrCriticalStrikeDamage { get; set; } // 치명타 데미지

        float CurrArmorPenetration { get; set; } // 방어 관통력
        float CurrMagicPenetration { get; set; } // 마법 관통력

        float CurrActionSpeed { get; set; } // 행동 스피드*/
    }
}

