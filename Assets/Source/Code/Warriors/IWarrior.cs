using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Source.Code.Warriors
{
    public interface IWarrior
    {
        public Sprite Icon { get;}
        public CharacterTypeId TypeId {get;}    
        public WarriorState State {get;}
        public IdleNumber Health {get;}
        public IdleNumber MaxHealth { get;}
        public IdleNumber BaseDamagePerSecond {get;}
        public float BaseNormalizedSpeed {get;}
        public Vector2 NormalizePosition {get;}
        WarriorBooster Booster { get; }
    }
}