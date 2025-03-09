using UnityEngine;

namespace Source.Code.StaticData
{
    public class BossConfig
    {
        [field: SerializeField] public int Stage { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Hp { get; private set; }
        [field: SerializeField] public int DamagePerSecond { get; private set; }
        
    }
}