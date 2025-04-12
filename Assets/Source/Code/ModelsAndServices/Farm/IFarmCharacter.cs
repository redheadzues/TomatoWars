using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.ModelsAndServices.Farm
{
    public interface IFarmCharacter
    {
        public CharacterTypeId TypeId { get; }
        public Sprite Icon { get; }
        public int Level { get; }
        public IdleNumber Cost { get; }
        public IdleNumber IncomePerSecond { get; }
        public float IncomeTime { get; }
    }
}