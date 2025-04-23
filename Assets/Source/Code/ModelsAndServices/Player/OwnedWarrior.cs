using System;
using Source.Code.StaticData;
using Source.Code.Warriors;

namespace Source.Code.ModelsAndServices.Player
{
    [Serializable]
    public class OwnedWarrior : IOwnedWarrior
    {
        public CharacterTypeId TypeId { get; set; }
        public Rarity Rarity { get; set; }
        public Booster BoosterInfo { get; set; } = new(BoosterTypeId.None);
        public int Level { get; set; }
        public int ShardsCount { get; set; }
        public int RequiredShardsToNextLevel { get; set; }
        
    }
}