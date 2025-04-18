using Source.Code.StaticData;

namespace Source.Code.ModelsAndServices.Player
{
    public interface IOwnedWarrior
    {
        public CharacterTypeId TypeId { get; }
        public bool IsOwned { get; }
        public Booster BoosterInfo { get; }
        public int Level { get; }

    }
}