using System;
using System.Collections.Generic;
using Source.Code.Grid;

namespace Source.Code.Models
{
    public interface IMergeGridModel
    {
        IReadOnlyList<GridBooster> GridBoosters { get; }
    }

    [Serializable]
    public class GridModel : IMergeGridModel
    {
        public List<GridBooster> GridBoosters { get; set; }
        public int MergeCount;
        IReadOnlyList<GridBooster> IMergeGridModel.GridBoosters => GridBoosters;
        
    }
}