using System;
using Source.Code.StaticData;
using Source.Code.Warriors;

namespace Source.Code.ModelsAndServices.Player
{
    public interface IOwnedWarrior
    {
        public CharacterTypeId TypeId { get; }
        public bool IsOwned { get; }
        public Booster BoosterInfo { get; }
        public int Level { get; }

    }
    
    [Serializable]
    public class OwnedWarrior : IOwnedWarrior
    {
        public CharacterTypeId TypeId { get; set; }
        public bool IsOwned { get; set; }
        public Booster BoosterInfo { get; set; } = new(BoosterTypeId.None);
        public int Level { get; set; }
        
    }
}