using System;
using Source.Code.IdleNumbers;
using UnityEngine;

namespace Source.Code.StaticData
{
    [Serializable]
    public class FarmCharacterConfig
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public CharacterTypeId TypeId { get; private set; }
        [field: SerializeField] public int FarmValue { get; private set; }
        [field: SerializeField] public int StartCost { get; private set; }

        public IdleNumber GetFarmValueByLevel(int level) => 
            FarmValue * level;

        public IdleNumber GetCostByLevel(int level) => 
            StartCost * (1 + level);
    }
}