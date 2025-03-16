using System;
using System.Collections.Generic;
using Source.Code.StaticData;

namespace Source.Code.Models.Player
{
    [Serializable]
    public class OwnedWarriors
    {
        public WarriorTypeId TypeId;
        public bool IsOwned;
        public WarriorBooster Booster;
    }
}