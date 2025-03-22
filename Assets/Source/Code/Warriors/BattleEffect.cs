namespace Source.Code.Warriors
{
    public class BattleEffect
    {
        public float Duration { get; }
        public float StartTime { get; }
        public float Value { get; }
        public EffectType TypeId { get;  }
        public IWarrior Source { get; }

        public BattleEffect(float duration, float startTime, float value, EffectType typeId, IWarrior source)
        {
            Duration = duration;
            StartTime = startTime;
            Value = value;
            TypeId = typeId;
            Source = source;
        }
    }

    public enum EffectType
    {
        Heal,
        Speed,
    }
}