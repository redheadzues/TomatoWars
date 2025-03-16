using System;
using Source.Code.StaticData;

namespace Source.Code.ModelsAndServices.Player
{
    [Serializable]
    public class OwnedWarriors
    {
        public WarriorTypeId TypeId;
        public bool IsOwned;
        public WarriorBooster Booster;
    }
}