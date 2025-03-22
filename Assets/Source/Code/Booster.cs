using Source.Code.StaticData;

namespace Source.Code
{
    public class Booster
    {
        public BoosterTypeId TypeId { get;}
        public int Level { get; }
        public Rarity Rarity { get; }

        public Booster(BoosterTypeId typeId, int level = 1, Rarity rarity = Rarity.Common)
        {
            TypeId = typeId;
            Level = level;
            Rarity = rarity;
        }
    }
}