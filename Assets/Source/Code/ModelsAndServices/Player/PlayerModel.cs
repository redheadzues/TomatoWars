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

        public List<CharacterTypeId> SelectedWarrior = new ()
        {
            CharacterTypeId.Tomato,
            CharacterTypeId.Potato
        };

        public Dictionary<CharacterTypeId, OwnedWarrior> OwnedWarriors = new();
    }
}