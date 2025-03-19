using System;
using Source.Code.ModelsAndServices.Grid;
using Source.Code.ModelsAndServices.Player;

namespace Source.Code.ModelsAndServices
{
    [Serializable]
    public class CoreModel
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