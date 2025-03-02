using System;
using UnityEngine;

namespace Source.Code.StaticData
{
    public enum WarriorType
    {
        Tomato,
        Potato
    }
    
    [Serializable]
    public class WarriorConfig
    { 
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }
        [field: SerializeField] public WarriorType Type { get; private set; }
    }
}