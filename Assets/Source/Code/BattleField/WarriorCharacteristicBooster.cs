namespace Source.Code.BattleField
{
    public class WarriorCharacteristicBooster
    {
        public int DamagePerSecond {get; private set;}
        public float NormalizedSpeed {get; private set;}
        public float CriticalChance {get; private set;}
        public float CriticalPower {get; private set;}
        public int MaxHealth { get; private set; }

        public WarriorCharacteristicBooster(int damagePerSecond = 0,
            float normalizedSpeed = 0,
            float criticalChance = 0,
            float criticalPower = 0,
            int maxHealth = 0)
        {
            DamagePerSecond = damagePerSecond;
            NormalizedSpeed = normalizedSpeed;
            CriticalChance = criticalChance;
            CriticalPower = criticalPower;
            MaxHealth = maxHealth;
        }
    }
}