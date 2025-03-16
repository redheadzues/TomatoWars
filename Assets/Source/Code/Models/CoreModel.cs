using System;
using Source.Code.Models.Player;

namespace Source.Code.Models
{
    [Serializable]
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