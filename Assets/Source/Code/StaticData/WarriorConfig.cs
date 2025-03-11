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
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public int DamagePerSecond { get; private set; }
        [field: SerializeField] public float NormalizedSpeed { get; private set; }
        [field: SerializeField] public WarriorTypeId TypeId { get; private set; }
    }

    public class Warrior : IWarrior
    {
        private static int _nextId;
        public int Id { get; }
        public Sprite Sprite { get; set; }
        public WarriorTypeId TypeId {get; set;}    
        public WarriorState State {get; set;}
        public int Health {get; set;}
        public int MaxHealth { get; set; }
        public int DamagePerSecond {get; set;}
        public float NormalizedSpeed {get; set;}
        public int LineIndex {get; set;}
        public float NormalizePosition {get; set;}

        public Warrior()
        {
            Id = _nextId++;
        }
    }

    public interface IWarrior
    {
        public int Id { get; } 
        public Sprite Sprite { get;}
        public WarriorTypeId TypeId {get;}    
        public WarriorState State {get;}
        public int Health {get;}
        public int MaxHealth { get;}
        public int DamagePerSecond {get;}
        public float NormalizedSpeed {get;}
        public int LineIndex {get;}
        public float NormalizePosition {get;}
    }
}