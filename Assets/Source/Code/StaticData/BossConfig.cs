using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Code.StaticData
{
    [Serializable]
    public class BossConfig
    {
        [field: SerializeField] public int Stage { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int Hp { get; private set; }
        [field: SerializeField] public BossAttackConfig AttackConfig { get; private set; }
        [field: SerializeField] public List<BossReward> Rewards { get; private set; }
    }

    [Serializable]
    public class BossReward
    {
        [field: SerializeField, Range(0, 1)] public float Treshold { get; private set; }
       
        [field: SerializeField] public CurrencyTypeId TypeId { get; private set; }
        [field: SerializeField] public int Value { get; private set; }
    }
}