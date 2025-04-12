using System;
using Source.Code.Warriors;
using UnityEngine;

namespace Source.Code.StaticData
{
    public enum CharacterTypeId
    {
        None,
        Tomato,
        Potato,
        Eggplant,
        Carrot,
        Broccoli,
        Beet
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
        [SerializeField] private int _health;
        [SerializeField] private float _criticalChance;
        [SerializeField] private float _maxCriticalChance;
        [SerializeField] private float _criticalPower;
        [SerializeField] private int _damagePerSecond;
        [SerializeField] private float _normalizedSpeed;
        [SerializeField] private float _damageReduce;
        [SerializeField] private float _maxDamageReduce;
        
        [field: SerializeField] public CharacterTypeId TypeId { get; private set; }
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