using System;
using System.Collections.Generic;
using Source.Code.IdleNumbers;
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
        public IdleNumber BaseDamagePerSecond => _stats.DamagePerSecond;
        public float BaseNormalizedSpeed => _stats.NormalizedSpeed;
        public float BaseCriticalChance => _stats.CriticalChance;
        public float BaseCriticalPower => _stats.CriticalPower;
        public float BaseDamageReduce => _stats.DamageReduce;
        public int LineIndex {get; set; }
        public float NormalizePosition {get; set; }
        public WarriorBooster Booster { get;}
        public List<BattleEffect> ActiveEffects { get;  }
       
        public Warrior(WarriorTypeId typeId, WarriorStats stats, WarriorBooster booster,  Sprite icon)
        {
            _stats = stats;
            Booster = booster;
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

        public IdleNumber CalculateDamage(float tickTime)
        {
            //to do implement critical damage
            
            return (BaseDamagePerSecond + Booster.DamagePerSecond) * tickTime;
        }

        public void TakeHeal(int value)
        {
            _health = Math.Clamp(_health + value, 0, MaxHealth);
        }

        private int CalculateMaxHealth()
        {
            if (Booster == null)
                return _baseMaxHealth;

            return _baseMaxHealth * Booster.MaxHealth;
        }
    }
}