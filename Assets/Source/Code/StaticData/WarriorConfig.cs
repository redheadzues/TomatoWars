using System;
using UnityEngine;

namespace Source.Code.StaticData
{
    public enum WarriorType
    {
        Tomato,
        Potato
    }

    public enum WarriorState
    {
        Walk,
        Fight, 
        Die
    }
    
    [Serializable]
    public class WarriorConfig
    { 
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public int DamagePerSecond { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public WarriorType Type { get; private set; }
    }

    public class Warrior
    {
        public WarriorType Type;
        public WarriorState State;
        public int Health;
        public int DamagePerSecond;
        public float Speed;
        public int LineIndex;
        public float NormalizePosition;

    }
}