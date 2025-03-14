using System.Collections.Generic;
using Source.Code.Grid;

namespace Source.Code.Models
{
    public interface IMergeGridModel
    {
        IReadOnlyList<GridBooster> GridBoosters { get; }
    }

    public class GridModel : IMergeGridModel
    {
        public List<GridBooster> GridBoosters { get; } = new List<GridBooster>(new GridBooster[30]);
        public int MergeCount;
        IReadOnlyList<GridBooster> IMergeGridModel.GridBoosters => GridBoosters;
        
    }
}