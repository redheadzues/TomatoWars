using System;
using UnityEngine;

namespace Source.Code.StaticData
{
    [Serializable]
    public class BossConfig
    {
        [field: SerializeField] public int Stage { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int Hp { get; private set; }
        [field: SerializeField] public int DamagePerSecond { get; private set; }
        
    }
}