using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.Warriors
{
    public interface IWarrior
    {
        public Sprite Icon { get;}
        public WarriorTypeId TypeId {get;}    
        public WarriorState State {get;}
        public int Health {get;}
        public int MaxHealth { get;}
        public int BaseDamagePerSecond {get;}
        public float BaseNormalizedSpeed {get;}
        public int LineIndex {get;}
        public float NormalizePosition {get;}
    }
}