using System;
using Source.Code.IdleNumbers;
using UnityEngine;

namespace Source.Code.StaticData
{
    [Serializable]
    public class BossAttackConfig
    {
        [field: SerializeField] public BossAttackTypeId TypeId { get; private set;}
        [field: SerializeField] public int DamagePerSecond  { get; private set;}
        [field: SerializeField] public float Cooldown { get; private set;}
        [field: SerializeField, Range(0f, 1f)] public float NormalizedWidth { get; private set;}
        [field: SerializeField, Min(1)] public int Count { get; private set;}
    }
}