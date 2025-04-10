using System;
using System.Collections.Generic;
using Source.Code.IdleNumbers;
using Source.Code.Warriors;
using UnityEngine;

namespace Source.Code.StaticData
{
    [Serializable]
    public class BoosterConfig
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public BoosterTypeId TypeId { get; private set; }
        [field: SerializeField] public List<EffectByRarity> EffectsByRarity { get; private set; }
        
        [Space] [Header("Stats")]
        [SerializeField] private int _damagePerSecond; 
        [SerializeField] private float _normalizedSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _criticalChance;
        [SerializeField] private float _maxCriticalChance;
        [SerializeField] private float _criticalPower; 
        [SerializeField] private int _maxHealth;

        public WarriorBooster GetStatsBoosterByLevel(int level, Rarity rarity)
        {
            var rarityMultiplier = 0.5f + ((int)rarity * 0.5f);
            
            IdleNumber damage = _damagePerSecond * level * rarityMultiplier;
            var speed = Math.Clamp(_normalizedSpeed * level * rarityMultiplier, 0, _maxSpeed);
            var criticalChance = Math.Clamp(_criticalChance * level * rarityMultiplier, 0, _maxCriticalChance);
            var criticalPower = _criticalChance * level * rarityMultiplier;
            IdleNumber maxHealth = _maxHealth * level * rarityMultiplier;

            return new WarriorBooster(TypeId, level, rarity, damage,speed, criticalChance, criticalPower, maxHealth);
        }
    }
        
    public enum BoosterTypeId
    {
        None, 
        Sword,
        Axe,
        Shield,
        Health,
    }

    [Serializable]
    public class EffectByRarity
    {
        [field: SerializeField] public Rarity Rarity { get; private set; }
        [SerializeField] private List<EffectType> _effectType;

        public IReadOnlyList<EffectType> EffectTypes => _effectType;
    }
}