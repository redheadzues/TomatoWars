using System;
using System.Collections.Generic;
using Source.Code.BattleField;
using Source.Code.ModelsAndServices.Player;
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
        private readonly PlayerModel _playerModel;
        private readonly IWarriorStatsService _warriorStats;
        private readonly IStaticDataService _staticData;
        private readonly Dictionary<WarriorTypeId, WarriorCharacteristicBooster> _boostersByWarriorType = new();

        public WarriorFactory(PlayerModel playerModel, IWarriorStatsService warriorStats, IStaticDataService staticData)
        {
            _playerModel = playerModel;
            _warriorStats = warriorStats;
            _staticData = staticData;
        }

        public Warrior CreateWarrior(WarriorTypeId typeId)
        {
            var warriorStats = _warriorStats.GetStatsByType(typeId);
            var icon = _staticData.GetWarriorConfig(typeId)?.Icon;
            
            var warrior = new Warrior(warriorStats, typeId, icon)
            {
                Booster = _boostersByWarriorType.GetValueOrDefault(typeId)  ?? new WarriorCharacteristicBooster(),
                BoosterInfo = _playerModel.OwnedWarriors.GetValueOrDefault(typeId)?.Booster
            };

            return warrior;

        }
    }
}