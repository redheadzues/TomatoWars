using Source.Code.BattleField;

namespace Source.Code.Models
{
    public class CoreModel : IReadOnlyCoreModel
    {
        public PlayerModel Player { get; set; }
        public GridModel Grid { get; set; }

        public CoreModel()
        {
            Player = new();
            Grid = new();
        }
    }
    
}