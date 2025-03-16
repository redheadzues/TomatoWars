using System;
using UnityEngine;

namespace Source.Code.StaticData
{
    public enum WarriorTypeId
    {
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
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public int DamagePerSecond { get; private set; }
        [field: SerializeField] public float NormalizedSpeed { get; private set; }
        [field: SerializeField] public WarriorTypeId TypeId { get; private set; }
    }

    public class Warrior : IWarrior
    {
        private int _health;
        
        public Sprite Icon { get; }
        public WarriorTypeId TypeId {get;}
        public WarriorState State { get; set; }
        public int Health => _health;
        public int MaxHealth { get; }
        public int BaseDamagePerSecond {get; }
        public float NormalizedSpeed {get;}
        public int LineIndex {get; set; }
        public float NormalizePosition {get; set; }

        public virtual int DamagePerSecond => BaseDamagePerSecond;
        
        public Warrior(WarriorConfig config)
        {
            Icon = config.Icon;
            TypeId = config.TypeId;
            _health = config.Health;
            MaxHealth = config.Health;
            BaseDamagePerSecond = config.DamagePerSecond;
            NormalizedSpeed = config.NormalizedSpeed;
        }

        public void ResetWarrior()
        {
            _health = MaxHealth;
            State = WarriorState.Walk;
            NormalizePosition = 0;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
        }
    }

    public interface IWarrior
    {
        public Sprite Icon { get;}
        public WarriorTypeId TypeId {get;}    
        public WarriorState State {get;}
        public int Health {get;}
        public int MaxHealth { get;}
        public int BaseDamagePerSecond {get;}
        public float NormalizedSpeed {get;}
        public int LineIndex {get;}
        public float NormalizePosition {get;}
    }
}