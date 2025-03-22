using System;
using Source.Code.StaticData;
using Source.Code.Warriors;

namespace Source.Code.ModelsAndServices.Player
{
    [Serializable]
    public class OwnedWarrior
    {
        public WarriorTypeId TypeId;
        public bool IsOwned;
        public Booster BoosterInfo { get; set; }
        public int Level { get; set; }
        
    }
}