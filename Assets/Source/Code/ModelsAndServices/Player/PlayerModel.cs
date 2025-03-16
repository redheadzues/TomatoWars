using System;
using System.Collections.Generic;
using Source.Code.StaticData;

namespace Source.Code.ModelsAndServices.Player
{
    [Serializable]
    public class PlayerModel
    {
        public Wallet Wallet = new();
        public int Stage { get; set; } = 1;

        public List<WarriorTypeId> SelectedWarrior = new ()
        {
            WarriorTypeId.Tomato,
            WarriorTypeId.Potato
        };

        public Dictionary<WarriorTypeId, OwnedWarrior> OwnedWarriors = new();

    }
}