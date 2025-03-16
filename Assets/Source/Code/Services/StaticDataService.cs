using System.Collections.Generic;
using System.Linq;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.Services
{
    public class StaticDataService : Service
    {
        private Dictionary<WarriorTypeId, WarriorConfig> _warriors;
        private Dictionary<int, BossConfig> _bosses;
        private Dictionary<BoosterTypeId, BoosterConfig> _boosters;
    
        public void LoadData()
        {
            _warriors = Resources.Load<WarriorsList>("StaticData/WarriorConfigList").Configs.ToDictionary(x => x.TypeId, x => x);
            _bosses = Resources.Load<BossList>("StaticData/BossConfigList").Configs.ToDictionary(x => x.Stage, x => x);
            _boosters = Resources.Load<BoosterList>("StaticData/BoosterList").Configs.ToDictionary(x => x.TypeId, x => x);
        }

        public WarriorConfig GetWarrior(WarriorTypeId typeId) =>
            _warriors.GetValueOrDefault(typeId);

        public BossConfig GetBoss(int stage) =>
            _bosses.GetValueOrDefault(stage);

        public BoosterConfig GetBooster(BoosterTypeId typeId) =>
            _boosters.GetValueOrDefault(typeId);
    }
}