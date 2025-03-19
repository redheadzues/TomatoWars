using System;
using Source.Code.BattleField;
using UnityEngine;

namespace Source.Code.StaticData
{
    [Serializable]
    public class BoosterConfig
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public BoosterTypeId TypeId { get; private set; }
        
        [SerializeField] private int _damagePerSecond; 
        [SerializeField] private float _normalizedSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _criticalChance;
        [SerializeField] private float _maxCriticalChance;
        [SerializeField] private float _criticalPower; 
        [SerializeField] private int _maxHealth;

        public WarriorCharacteristicBooster GetCharacteristicBoosterByLevel(int level)
        {
            var damage = _damagePerSecond * level;
            var speed = Math.Clamp(_normalizedSpeed * level, 0, _maxSpeed);
            var criticalChance = Math.Clamp(_criticalChance * level, 0, _maxCriticalChance);
            var criticalPower = _criticalChance * level;
            var maxHealth = _maxHealth * level;

            return new WarriorCharacteristicBooster(damage, speed, criticalChance, criticalPower, maxHealth);
        }
    }
        
    public enum BoosterTypeId
    {
        None, 
        Sword,
        Axe,
        Shield,
        Heal,
    }
}