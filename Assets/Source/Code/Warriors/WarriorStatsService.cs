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
        WarriorStatsBooster GetBoosterByType(WarriorTypeId typeId);
    }
    
    public class WarriorStatsService : IWarriorStatsService
    {
        private readonly Dictionary<WarriorTypeId, WarriorStats> _warriorStatsByType = new();
        private readonly Dictionary<WarriorTypeId, WarriorStatsBooster> _boostersByType = new();
        private readonly IStaticDataService _staticData;
        private readonly PlayerModel _model;

        public WarriorStatsService(IStaticDataService staticData, PlayerModel model)
        {
            _staticData = staticData;
            _model = model;
            
            InitializeStats();
            InitializeBoosters();
        }

        public void CleanUp()
        {
            foreach (var warrior in _model.OwnedWarriors.Values)
            {
                warrior.BoosterUpdated -= OnBoosterUpdated;
                warrior.LevelUp -= OnLevelUp;
            }
        }

        public WarriorStats GetStatsByType(WarriorTypeId typeId) => 
            _warriorStatsByType.GetValueOrDefault(typeId);

        public WarriorStatsBooster GetBoosterByType(WarriorTypeId typeId) => 
            _boostersByType.GetValueOrDefault(typeId) ?? new WarriorStatsBooster();

        private void InitializeStats()
        {
            foreach (WarriorTypeId typeId in Enum.GetValues(typeof(WarriorTypeId)))
            {
                if (typeId == WarriorTypeId.None) 
                    continue;

                AddStatsByType(typeId);
            }
        }

        private void InitializeBoosters()
        {
            foreach (var warrior in _model.OwnedWarriors)
            {
                warrior.Value.BoosterUpdated += OnBoosterUpdated;
                warrior.Value.LevelUp += OnLevelUp;
                
                if (warrior.Value.IsOwned == false)
                    return;
                
                AddBoosterByType(warrior.Key, warrior.Value.Booster);
            }
        }

        private void AddBoosterByType(WarriorTypeId typeId, WarriorBooster booster)
        {
            if (booster == null || booster.TypeId == BoosterTypeId.None)
                return;
            
            var boosterConfig = _staticData.GetBoosterConfig(booster.TypeId);
            var warriorBooster = boosterConfig.GetStatsBoosterByLevel(booster.Level, booster.Rarity);

            _boostersByType[typeId] = warriorBooster;
        }
        
        private void AddStatsByType(WarriorTypeId typeId)
        {
            var config = _staticData.GetWarriorConfig(typeId);
            var level = _model.OwnedWarriors[typeId].Level;
            var stats = config.GetStatsByLevel(level);

            _warriorStatsByType[typeId] = stats;
        }


        private void OnBoosterUpdated(WarriorTypeId typeId, WarriorBooster booster) => 
            AddBoosterByType(typeId, booster);

        private void OnLevelUp(WarriorTypeId typeId) => 
            AddStatsByType(typeId);
    }
}