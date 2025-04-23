using Source.Code.StaticData;

namespace Source.Code.ModelsAndServices.Player
{
    public interface IOwnedWarrior
    {
        public CharacterTypeId TypeId { get; }
        public Rarity Rarity { get; }
        public Booster BoosterInfo { get; }
        public int Level { get; }
        public int ShardsCount { get; }
        public int RequiredShardsToNextLevel { get; }

    }
}