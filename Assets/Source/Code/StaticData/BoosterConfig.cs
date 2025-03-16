using System;
using UnityEngine;

namespace Source.Code.StaticData
{
    [Serializable]
    public class BoosterConfig
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public BoosterTypeId TypeId { get; private set; }
        
    }

    public enum BoosterTypeId
    {
        None, //None typeId should always be first
        Sword,
        Axe,
        Shield,
        Heal,
    }
}