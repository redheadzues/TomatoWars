using System;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.Grid
{
    [Serializable]
    public class GridBooster
    {
        public int Index { get; }
        public int Level { get; }
        public BoosterTypeId TypeId { get;}
        public Rarity Rarity { get; }
        public Sprite Icon { get; }
        
        
        public GridBooster(int index, BoosterTypeId typeId, int level, Sprite icon)
        {
            Index = index;
            TypeId = typeId;
            Level = level;
            Icon = icon;
        }

        public GridBooster(int index)
        {
            Index = index;
            TypeId = BoosterTypeId.None;
        }
    }
}