using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.ModelsAndServices.Farm
{
    public class FarmCharacter : IFarmCharacter
    {
        public CharacterTypeId TypeId { get; set; }
        public Sprite Icon { get; set; }
        public int Level { get; set; }
        public IdleNumber Cost { get; set; }
        public IdleNumber IncomePerSecond { get; set; }
        public float IncomeTime { get; set; }
        public float RemainingTimeToIncome { get; set; }

        public FarmCharacter(CharacterTypeId typeId, Sprite icon, int level, IdleNumber cost, IdleNumber incomePerSecond, float incomeTime)
        {
            TypeId = typeId;
            Icon = icon;
            Level = level;
            Cost = cost;
            IncomePerSecond = incomePerSecond;
            IncomeTime = incomeTime;
            RemainingTimeToIncome = IncomeTime;
        }
    }
}