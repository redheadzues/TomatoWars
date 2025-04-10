using Source.Code.StaticData;
using Source.Code.Warriors;

namespace Source.Code.ModelsAndServices
{
    public interface IWarriorFactory : IService
    {
        Warrior CreateWarrior(CharacterTypeId typeId);
    }
    
    public class WarriorFactory : IWarriorFactory
    {
        private readonly IWarriorStatsService _warriorStats;
        private readonly IStaticDataService _staticData;

        public WarriorFactory(IWarriorStatsService warriorStats, IStaticDataService staticData)
        {
            _warriorStats = warriorStats;
            _staticData = staticData;
        }

        public Warrior CreateWarrior(CharacterTypeId typeId)
        {
            var icon = _staticData.GetWarriorConfig(typeId)?.Icon;

            var warriorStats = _warriorStats.GetStatsByType(typeId);
            var statsBooster = _warriorStats.GetWarriorBoosterByType(typeId);

            var warrior = new Warrior(typeId, warriorStats, statsBooster, icon);

            return warrior;
        }
    }
}