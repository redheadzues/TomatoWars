using System;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.Grid
{
    [Serializable]
    public class GridBooster : Booster
    {
        public int Index { get; }
        public Sprite Icon { get; }
        
        
        public GridBooster(int index, BoosterTypeId typeId, int level, Sprite icon, Rarity rarity = Rarity.Common) : base(typeId, level, rarity)
        {
            Index = index;
            Icon = icon;
        }

        public GridBooster(int index) : base(BoosterTypeId.None)
        {
            Index = index;
        }
    }
}