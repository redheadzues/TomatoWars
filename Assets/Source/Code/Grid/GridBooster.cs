using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.Grid
{
    public class GridBooster
    {
        public int Index { get; }
        public int Level { get; }
        public GridBoosterTypeId TypeId { get;}
        public Sprite Icon { get; } 

        public GridBooster(int index, GridBoosterTypeId typeId, int level, Sprite icon)
        {
            Index = index;
            TypeId = typeId;
            Level = level;
            Icon = icon;
        }
    }
}