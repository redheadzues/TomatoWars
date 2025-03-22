using Source.Code.StaticData;
using Source.Code.Warriors;

namespace Source.Code.ModelsAndServices
{
    public interface IWarriorFactory : IService
    {
        Warrior CreateWarrior(WarriorTypeId typeId);
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

        public Warrior CreateWarrior(WarriorTypeId typeId)
        {
            var warriorStats = _warriorStats.GetStatsByType(typeId);
            var icon = _staticData.GetWarriorConfig(typeId)?.Icon;

            var statsBooster = _warriorStats.GetStatsBoosterByType(typeId);
            var boosterInfo = _warriorStats.GetBoosterInfoByType(typeId);

            var warrior = new Warrior(typeId, warriorStats, statsBooster, boosterInfo, icon);

            return warrior;
        }
    }
}