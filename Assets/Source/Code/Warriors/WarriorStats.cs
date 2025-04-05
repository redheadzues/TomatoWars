using Source.Code.IdleNumbers;

namespace Source.Code.Warriors
{
    public class WarriorStats
    {
        public IdleNumber MaxHealth {get; set; }
        public IdleNumber DamagePerSecond {get; set; }
        public float NormalizedSpeed {get; set; }
        public float CriticalChance { get; set; }
        public float CriticalPower { get; set; }
        public float DamageReduce { get; set; }
    }
}