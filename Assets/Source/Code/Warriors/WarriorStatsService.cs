using System;
using System.Collections.Generic;
using Source.Code.ModelsAndServices;
using Source.Code.ModelsAndServices.Farm;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;

namespace Source.Code.Warriors
{
    public interface IWarriorStatsService : IService, ICleanable
    {
        WarriorStats GetStatsByType(CharacterTypeId typeId);
        WarriorBooster GetWarriorBoosterByType(CharacterTypeId typeId);
    }
    
    public class WarriorStatsService : IWarriorStatsService
    {
        private readonly Dictionary<CharacterTypeId, WarriorStats> _warriorStatsByType = new();
        private readonly Dictionary<CharacterTypeId, WarriorBooster> _statsBoostersByType = new();
        private readonly IStaticDataService _staticData;
        private readonly IPlayerService _playerService;
        private readonly IFarmService _farmService;

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

        public WarriorStats GetStatsByType(CharacterTypeId typeId) => 
            _warriorStatsByType.GetValueOrDefault(typeId);

        public WarriorBooster GetWarriorBoosterByType(CharacterTypeId typeId) => 
            _statsBoostersByType.GetValueOrDefault(typeId);

        private void InitializeStats()
        {
            foreach (CharacterTypeId typeId in Enum.GetValues(typeof(CharacterTypeId)))
            {
                if (typeId == CharacterTypeId.None) 
                    continue;

                AddStatsByType(typeId);
            }
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
        
        private void AddStatsByType(CharacterTypeId typeId)
        {
            var farmLevel = _farmService.CharactersLevel[typeId];
            
            var config = _staticData.GetWarriorConfig(typeId);
            var level = _playerService.Model.OwnedWarriors[typeId].Level;
            var stats = config.GetStatsByLevel(level, farmLevel);

            _warriorStatsByType[typeId] = stats;
        }

        private void AddBoosterByType(CharacterTypeId typeId, Booster boosterInfo)
        {
            if (boosterInfo.TypeId == BoosterTypeId.None)
            {
                _statsBoostersByType[typeId] = new WarriorBooster(BoosterTypeId.None);
                return;
            }
            
            var boosterConfig = _staticData.GetBoosterConfig(boosterInfo.TypeId);
            var warriorBooster = boosterConfig.GetStatsBoosterByLevel(boosterInfo.Level, boosterInfo.Rarity);

            _statsBoostersByType[typeId] = warriorBooster;
        }

        private void OnBoosterUpdated(CharacterTypeId typeId, Booster booster) => 
            AddBoosterByType(typeId, booster);

        private void OnLevelUp(CharacterTypeId typeId) => 
            AddStatsByType(typeId);
    }
}