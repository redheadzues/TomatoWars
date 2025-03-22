using System;
using System.Collections.Generic;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.Warriors
{
    public class Warrior : IWarrior
    {
        private int _baseMaxHealth;
        private int _health;
        private WarriorStats _stats;
        public Sprite Icon { get; }
        public WarriorTypeId TypeId {get;}
        public WarriorState State { get; set; }
        public int Health => _health;
        public int MaxHealth => CalculateMaxHealth();
        public int BaseDamagePerSecond => _stats.DamagePerSecond;
        public float BaseNormalizedSpeed => _stats.NormalizedSpeed;
        public float BaseCriticalChance => _stats.CriticalChance;
        public float BaseCriticalPower => _stats.CriticalPower;
        public float BaseDamageReduce => _stats.DamageReduce;
        public int LineIndex {get; set; }
        public float NormalizePosition {get; set; }
        public WarriorBoosterInfo BoosterInfo { get; }
        public WarriorStatsBooster StatsBooster { get;}
        public List<BattleEffect> ActiveEffects { get;  }
       
        public Warrior(WarriorTypeId typeId, WarriorStats stats, WarriorStatsBooster statsBooster, WarriorBoosterInfo boosterInfo, Sprite icon)
        {
            _stats = stats;
            StatsBooster = statsBooster;
            BoosterInfo = boosterInfo;
            Icon = icon;
            TypeId = typeId;
            _health = stats.MaxHealth;
            _baseMaxHealth = stats.MaxHealth;
        }

        public void ResetWarrior()
        {
            _health = CalculateMaxHealth();
            State = WarriorState.Walk;
            NormalizePosition = 0;
        }

        public void TakeDamage(int damage)
        {
            _health -= (int)(damage * (1- BaseDamageReduce));

            if (_health <= 0)
                State = WarriorState.Died;
        }

        public int CalculateDamage(float tickTime)
        {
            //to do implement critical damage
            
            return (int)((BaseDamagePerSecond + StatsBooster.DamagePerSecond) * tickTime);
        }

        public void TakeHeal(int value)
        {
            _health = Math.Clamp(_health + value, 0, MaxHealth);
        }

        private int CalculateMaxHealth()
        {
            if (StatsBooster == null)
                return _baseMaxHealth;

            return _baseMaxHealth * StatsBooster.MaxHealth;
        }
    }
}