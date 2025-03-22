using System;
using System.Collections.Generic;
using Source.Code.ModelsAndServices;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;

namespace Source.Code.Warriors
{
    public interface IWarriorStatsService : IService, ICleanable
    {
        WarriorStats GetStatsByType(WarriorTypeId typeId);
        WarriorBooster GetWarriorBoosterByType(WarriorTypeId typeId);
    }
    
    public class WarriorStatsService : IWarriorStatsService
    {
        private readonly Dictionary<WarriorTypeId, WarriorStats> _warriorStatsByType = new();
        private readonly Dictionary<WarriorTypeId, WarriorBooster> _statsBoostersByType = new();
        private readonly IStaticDataService _staticData;
        private readonly IPlayerService _playerService;

        public WarriorStatsService(IStaticDataService staticData, IPlayerService playerService)
        {
            _staticData = staticData;
            _playerService = playerService;
            
            InitializeStats();
            InitializeBoosters();

            _playerService.WarriorLevelUp += OnLevelUp;
            _playerService.BoosterUpdated += OnBoosterUpdated;
        }

        public void CleanUp()
        {
            _playerService.WarriorLevelUp -= OnLevelUp;
            _playerService.BoosterUpdated -= OnBoosterUpdated;
        }

        public WarriorStats GetStatsByType(WarriorTypeId typeId) => 
            _warriorStatsByType.GetValueOrDefault(typeId);

        public WarriorBooster GetWarriorBoosterByType(WarriorTypeId typeId) => 
            _statsBoostersByType.GetValueOrDefault(typeId);

        private void InitializeStats()
        {
            foreach (WarriorTypeId typeId in Enum.GetValues(typeof(WarriorTypeId)))
            {
                if (typeId == WarriorTypeId.None) 
                    continue;

                AddStatsByType(typeId);
            }
        }
        
        private void AddStatsByType(WarriorTypeId typeId)
        {
            var config = _staticData.GetWarriorConfig(typeId);
            var level = _playerService.Model.OwnedWarriors[typeId].Level;
            var stats = config.GetStatsByLevel(level);

            _warriorStatsByType[typeId] = stats;
        }

        private void InitializeBoosters()
        {
            foreach (var warrior in _playerService.Model.OwnedWarriors)
            {
                if (warrior.Value.IsOwned == false)
                    return;
                
                AddBoosterByType(warrior.Key, warrior.Value.BoosterInfo);
            }
        }

        private void AddBoosterByType(WarriorTypeId typeId, Booster boosterInfo)
        {
            if (boosterInfo.TypeId == BoosterTypeId.None)
                return;
            
            var boosterConfig = _staticData.GetBoosterConfig(boosterInfo.TypeId);
            var warriorBooster = boosterConfig.GetStatsBoosterByLevel(boosterInfo.Level, boosterInfo.Rarity);

            _statsBoostersByType[typeId] = warriorBooster;
        }

        private void OnBoosterUpdated(WarriorTypeId typeId, Booster booster) => 
            AddBoosterByType(typeId, booster);

        private void OnLevelUp(WarriorTypeId typeId) => 
            AddStatsByType(typeId);
    }
}