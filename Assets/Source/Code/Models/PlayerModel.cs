using System.Collections.Generic;
using Source.Code.StaticData;

namespace Source.Code.Models
{
    public interface IReadOnlyPlayerModel
    {
        int Gold { get;}
        int Gem { get;}
        int Stage { get;}
    }
    
    public class PlayerModel : IReadOnlyPlayerModel
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