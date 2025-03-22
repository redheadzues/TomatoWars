using System;
using Source.Code.StaticData;

namespace Source.Code.ModelsAndServices.Player
{
    [Serializable]
    public class OwnedWarrior
    {
        public WarriorTypeId TypeId;
        public bool IsOwned;
        public WarriorBoosterInfo BoosterInfo { get; set; }
        public int Level { get; set; }
        
    }
}