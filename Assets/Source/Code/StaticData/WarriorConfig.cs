using System;
using Source.Code.Warriors;
using UnityEngine;

namespace Source.Code.StaticData
{
    public enum WarriorTypeId
    {
        None,
        Tomato,
        Potato
    }

    public enum WarriorState
    {
        None,
        Walk,
        Fight, 
        Died
    }
    
    [Serializable]
    public class WarriorConfig
    { 
        [SerializeField] public int _health;
        [SerializeField] public float _criticalChance;
        [SerializeField] public float _maxCriticalChance;
        [SerializeField] public float _criticalPower;
        [SerializeField] public int _damagePerSecond;
        [SerializeField] public float _normalizedSpeed;
        [SerializeField] private float _damageReduce;
        [SerializeField] private float _maxDamageReduce;
        
        [field: SerializeField] public WarriorTypeId TypeId { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        public WarriorStats GetStatsByLevel(int level)
        {
            if (level == 0)
                level = 1;
            
            var stats = new WarriorStats()
            {
                MaxHealth = _health,
                CriticalChance = Math.Clamp(_criticalChance * level, 0, _maxCriticalChance),
                CriticalPower = _criticalPower * level,
                DamagePerSecond = _damagePerSecond * level,
                NormalizedSpeed = _normalizedSpeed,
                DamageReduce = Math.Clamp(_damageReduce * level, 0, _maxDamageReduce),
                
            };
            
            return stats;
        }
    }
}