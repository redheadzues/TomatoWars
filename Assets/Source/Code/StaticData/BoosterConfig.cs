using System;
using UnityEngine;

namespace Source.Code.StaticData
{
    [Serializable]
    public class BoosterConfig
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public GridBoosterTypeId TypeId { get; private set; }
    }

    public enum GridBoosterTypeId
    {
        None, //None typeId should always be first
        Sword,
        Axe,
        Shield,
        Heal,
    }
}