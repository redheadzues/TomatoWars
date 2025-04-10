using System;
using System.Collections.Generic;
using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.Warriors
{
    public class Warrior : IWarrior
    {
        private IdleNumber _baseMaxHealth;
        private IdleNumber _currentHealth;
        private WarriorStats _stats;
        public Sprite Icon { get; }
        public WarriorTypeId TypeId {get;}
        public WarriorState State { get; set; }
        public IdleNumber Health => _currentHealth;
        public IdleNumber MaxHealth => CalculateMaxHealth();
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
            _currentHealth = stats.MaxHealth;
            _baseMaxHealth = stats.MaxHealth;
        }

        public void ResetWarrior()
        {
            _currentHealth = CalculateMaxHealth();
            State = WarriorState.Walk;
            NormalizePosition = 0;
        }

        public void TakeDamage(IdleNumber damage)
        {
            _currentHealth -= damage * (1- BaseDamageReduce);
            
            if (_currentHealth <= 0)
                State = WarriorState.Died;
        }

        public IdleNumber CalculateDamage(float tickTime)
        {
            //to do implement critical damage
            
            return (BaseDamagePerSecond + Booster.DamagePerSecond) * tickTime;
        }

        public void TakeHeal(IdleNumber value)
        {
            _currentHealth += value;

            if (_currentHealth > _baseMaxHealth)
                _currentHealth = _baseMaxHealth;
        }

        private IdleNumber CalculateMaxHealth()
        {
            if (Booster == null || Booster.MaxHealth == 0)
                return _baseMaxHealth;

            return _baseMaxHealth * Booster.MaxHealth;
        }
    }
}