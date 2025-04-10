using Source.Code.IdleNumbers;
using Source.Code.StaticData;

namespace Source.Code.Warriors
{
    public class WarriorBooster : Booster
    {
        public IdleNumber DamagePerSecond {get; private set;}
        public float NormalizedSpeed {get; private set;}
        public float CriticalChance {get; private set;}
        public float CriticalPower {get; private set;}
        public IdleNumber MaxHealth { get; private set; }

        public WarriorBooster(BoosterTypeId typeId, int level = 1, Rarity rarity = Rarity.Common,
            IdleNumber damagePerSecond = default,
            float normalizedSpeed = 0,
            float criticalChance = 0,
            float criticalPower = 0,
            IdleNumber maxHealth = default) : base(typeId, level, rarity)
        {
            DamagePerSecond = damagePerSecond;
            NormalizedSpeed = normalizedSpeed;
            CriticalChance = criticalChance;
            CriticalPower = criticalPower;
            MaxHealth = maxHealth;
        }
    }
}