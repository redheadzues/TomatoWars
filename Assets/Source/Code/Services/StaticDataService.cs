using System.Collections.Generic;
using System.Linq;
using Source.Code.Services;
using Source.Code.StaticData;
using UnityEngine;

public class StaticDataService : Service
{
    private Dictionary<WarriorTypeId, WarriorConfig> _warriors;
    private Dictionary<int, BossConfig> _bosses;
    
    public void LoadData()
    {
        _warriors = Resources.Load<WarriorsList>("StaticData/WarriorConfigList").Configs.ToDictionary(x => x.TypeId, x => x);
        _bosses = Resources.Load<BossList>("StaticData/BossConfigList").Configs.ToDictionary(x => x.Stage, x => x);
        
    }

    public WarriorConfig GetWarrior(WarriorTypeId typeId) =>
        _warriors.TryGetValue(typeId, out WarriorConfig config)
            ? config : null;

    public BossConfig GetBoss(int stage) =>
        _bosses.TryGetValue(stage, out BossConfig config)
            ? config : null;
}