using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.ModelsAndServices.Farm
{
    public interface IFarmCharacter
    {
        CharacterTypeId TypeId { get; }
        Sprite Icon { get; }
        int Level { get; }
        IdleNumber Cost { get; }
        IdleNumber IncomePerSecond { get; }
        float IncomeTime { get; }
        float RemainingTimeToIncome { get; }
    }
}