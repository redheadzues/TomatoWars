using Source.Code.StaticData;

namespace Source.Code.Warriors
{
    public class WarriorBooster : Booster
    {
        public int DamagePerSecond {get; private set;}
        public float NormalizedSpeed {get; private set;}
        public float CriticalChance {get; private set;}
        public float CriticalPower {get; private set;}
        public int MaxHealth { get; private set; }

        public WarriorBooster(BoosterTypeId typeId, int level = 1, Rarity rarity = Rarity.Common,
            int damagePerSecond = 0,
            float normalizedSpeed = 0,
            float criticalChance = 0,
            float criticalPower = 0,
            int maxHealth = 0) : base(typeId, level, rarity)
        {
            DamagePerSecond = damagePerSecond;
            NormalizedSpeed = normalizedSpeed;
            CriticalChance = criticalChance;
            CriticalPower = criticalPower;
            MaxHealth = maxHealth;
        }
    }
}