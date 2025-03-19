using System;
using System.Collections.Generic;
using Source.Code.ModelsAndServices;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;

namespace Source.Code.Warriors
{
    public interface IWarriorStatsService : IService
    {
        WarriorStats GetStatsByType(WarriorTypeId typeId);
    }
    
    public class WarriorStatsService : IWarriorStatsService
    {
        private readonly Dictionary<WarriorTypeId, WarriorStats> _warriorStatsByType = new();
        private readonly IStaticDataService _staticData;
        private readonly PlayerModel _model;

        public WarriorStatsService(IStaticDataService staticData, PlayerModel model)
        {
            _staticData = staticData;
            _model = model;
            InitializeCharacteristics();
        }

        public WarriorStats GetStatsByType(WarriorTypeId typeId) => 
            _warriorStatsByType.GetValueOrDefault(typeId);
        
        private void InitializeCharacteristics()
        {
            foreach (WarriorTypeId typeId in Enum.GetValues(typeof(WarriorTypeId)))
            {
                if (typeId == WarriorTypeId.None) 
                    continue;
                
                var config = _staticData.GetWarriorConfig(typeId);
                var level = _model.OwnedWarriors[typeId].Level;
                var stats = config.GetStatsByLevel(level);

                _warriorStatsByType[typeId] = stats;
            }
        }
    }

    public class WarriorStats
    {
        public int MaxHealth {get; set; }
        public int DamagePerSecond {get; set; }
        public float NormalizedSpeed {get; set; }
        public float CriticalChance { get; set; }
        public float CriticalPower { get; set; }
        public float DamageReduce { get; set; }
    }
}