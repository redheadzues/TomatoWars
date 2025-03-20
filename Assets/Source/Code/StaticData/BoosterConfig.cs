using System;
using Source.Code.BattleField;
using Source.Code.Warriors;
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

        public WarriorStatsBooster GetStatsBoosterByLevel(int level, Rarity rarity)
        {
            var rarityMultiplier = 0.5f + ((int)rarity * 0.5f);
            
            var damage = (int)(_damagePerSecond * level * rarityMultiplier);
            var speed = Math.Clamp(_normalizedSpeed * level * rarityMultiplier, 0, _maxSpeed);
            var criticalChance = Math.Clamp(_criticalChance * level * rarityMultiplier, 0, _maxCriticalChance);
            var criticalPower = _criticalChance * level * rarityMultiplier;
            var maxHealth = (int)(_maxHealth * level * rarityMultiplier);

            return new WarriorStatsBooster(damage, speed, criticalChance, criticalPower, maxHealth);
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