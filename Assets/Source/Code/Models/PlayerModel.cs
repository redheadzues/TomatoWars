using System.Collections.Generic;
using Source.Code.StaticData;

namespace Source.Code.Models
{
    public class PlayerModel 
    {
        public int Gold { get; set; }
        public int Gem { get; set; }
        public int Stage  { get; set; }

        public List<WarriorTypeId> SelectedWarrior = new List<WarriorTypeId>()
        {
            WarriorTypeId.Tomato,
            WarriorTypeId.Tomato,
            WarriorTypeId.Tomato,
            WarriorTypeId.Tomato,
            WarriorTypeId.Potato
        };

        public PlayerModel()
        {
            Gold = 0;
            Gem = 0;
            Stage = 1;
        }
    }
}