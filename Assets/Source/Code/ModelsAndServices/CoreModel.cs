using System;
using Source.Code.ModelsAndServices.Farm;
using Source.Code.ModelsAndServices.Grid;
using Source.Code.ModelsAndServices.Player;

namespace Source.Code.ModelsAndServices
{
    [Serializable]
    public class CoreModel
    {
        public PlayerModel Player = new();
        public GridModel Grid = new();
        public FarmModel FarmModel = new();
    }
}