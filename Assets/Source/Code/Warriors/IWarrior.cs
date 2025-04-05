using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.Warriors
{
    public interface IWarrior
    {
        public Sprite Icon { get;}
        public WarriorTypeId TypeId {get;}    
        public WarriorState State {get;}
        public IdleNumber Health {get;}
        public IdleNumber MaxHealth { get;}
        public IdleNumber BaseDamagePerSecond {get;}
        public float BaseNormalizedSpeed {get;}
        public int LineIndex {get;}
        public float NormalizePosition {get;}
        WarriorBooster Booster { get; }
    }
}