using System;
using System.Collections.Generic;
using Source.Code.Grid;

namespace Source.Code.ModelsAndServices.Grid
{
    public interface IReadOnlyGridModel
    {
        IReadOnlyList<GridBooster> GridBoosters { get; }
    }

    [Serializable]
    public class GridModel : IReadOnlyGridModel
    {
        public List<GridBooster> GridBoosters { get; set; }
        public int BoostersCreated;
        IReadOnlyList<GridBooster> IReadOnlyGridModel.GridBoosters => GridBoosters;
        
    }
}