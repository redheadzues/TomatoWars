using System;
using System.Collections.Generic;
using System.Linq;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.ModelsAndServices
{
    public interface IStaticDataService : IService
    {
        event Action LoadCompleted;
        bool IsLoaded { get; }
        void LoadData();
        WarriorConfig GetWarriorConfig(WarriorTypeId typeId);
        BossConfig GetBossConfig(int stage);
        BoosterConfig GetBoosterConfig(BoosterTypeId typeId);
    }
    
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<WarriorTypeId, WarriorConfig> _warriors;
        private Dictionary<int, BossConfig> _bosses;
        private Dictionary<BoosterTypeId, BoosterConfig> _boosters;

        public bool IsLoaded { get; private set; }
        public event Action LoadCompleted;

        public void LoadData()
        {
            _warriors = Resources.Load<WarriorsList>("StaticData/WarriorConfigList").Configs.ToDictionary(x => x.TypeId, x => x);
            _bosses = Resources.Load<BossList>("StaticData/BossConfigList").Configs.ToDictionary(x => x.Stage, x => x);
            _boosters = Resources.Load<BoosterList>("StaticData/BoosterList").Configs.ToDictionary(x => x.TypeId, x => x);
            IsLoaded = true;
            LoadCompleted?.Invoke();
        }

        public WarriorConfig GetWarriorConfig(WarriorTypeId typeId) =>
            _warriors.GetValueOrDefault(typeId);

        public BossConfig GetBossConfig(int stage) =>
            _bosses.GetValueOrDefault(stage);

        public BoosterConfig GetBoosterConfig(BoosterTypeId typeId) =>
            _boosters.GetValueOrDefault(typeId);
    }
}