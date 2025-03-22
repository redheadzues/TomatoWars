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
        WarriorStatsBooster GetStatsBoosterByType(WarriorTypeId typeId);
        WarriorBoosterInfo GetBoosterInfoByType(WarriorTypeId typeId);
    }
    
    public class WarriorStatsService : IWarriorStatsService
    {
        private readonly Dictionary<WarriorTypeId, WarriorStats> _warriorStatsByType = new();
        private readonly Dictionary<WarriorTypeId, WarriorStatsBooster> _statsBoostersByType = new();
        private readonly Dictionary<WarriorTypeId, WarriorBoosterInfo> _boosterInfoByType = new();
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

        public WarriorStatsBooster GetStatsBoosterByType(WarriorTypeId typeId) => 
            _statsBoostersByType.GetValueOrDefault(typeId);

        public WarriorBoosterInfo GetBoosterInfoByType(WarriorTypeId typeId) =>
            _boosterInfoByType.GetValueOrDefault(typeId);

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

        private void AddBoosterByType(WarriorTypeId typeId, WarriorBoosterInfo boosterInfo)
        {
            if (boosterInfo.TypeId == BoosterTypeId.None)
                return;
            
            var boosterConfig = _staticData.GetBoosterConfig(boosterInfo.TypeId);
            var warriorStatsBooster = boosterConfig.GetStatsBoosterByLevel(boosterInfo.Level, boosterInfo.Rarity);

            _statsBoostersByType[typeId] = warriorStatsBooster;
            _boosterInfoByType[typeId] = boosterInfo;
        }

        private void OnBoosterUpdated(WarriorTypeId typeId, WarriorBoosterInfo boosterInfo) => 
            AddBoosterByType(typeId, boosterInfo);

        private void OnLevelUp(WarriorTypeId typeId) => 
            AddStatsByType(typeId);
    }
}