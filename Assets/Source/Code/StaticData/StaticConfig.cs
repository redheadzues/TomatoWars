using System.Collections.Generic;
using System.Numerics;

namespace Source.Code.StaticData
{
    public static class StaticConfig
    {
        public const float TICK_INTERVAL = 0.5f;
        public const int BOOSTER_LIMIT = 30;
        public const float VEGETABLES_SPAWN_TIME = 1.5f;
        public const int SHARDS_PACK_PRICE = 200;
        public const int SHARDS_PER_PACK = 10;

        private static Dictionary<Rarity, float> _chances = new()
        {
            { Rarity.Legendary, 0.01f },
            { Rarity.Epic, 0.1f },
            { Rarity.Rare, 0.4f },
            { Rarity.Common, 1 }
        };

        private static Dictionary<Rarity, Vector2> _shardsCountRangeByRarity = new()
        {
            { Rarity.Legendary, new Vector2(1, 3) },
            { Rarity.Epic, new Vector2(3, 10) },
            { Rarity.Rare, new Vector2(10, 20) },
            { Rarity.Common, new Vector2(30, 100) }
        };

        public static Rarity GetRarityByChance(float chance)
        {
            foreach (var keyValue in _chances)
            {
                if (chance <= keyValue.Value)
                    return keyValue.Key;
            }

            return Rarity.Common;
        }

        public static Vector2 GetShardsCountRangeByRarity(Rarity rarity)
        {
            return _shardsCountRangeByRarity[rarity];
        }
    }
}