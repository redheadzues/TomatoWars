using System.Collections.Generic;
using System.Linq;
using Source.Code.Services;
using Source.Code.StaticData;
using UnityEngine;

public class StaticDataService : Service
{
    private readonly Dictionary<WarriorTypeId, WarriorConfig> _warriors = new();
    private readonly Dictionary<int, BossConfig> _bosses = new();
    
    public void LoadData()
    {
        Resources.Load<WarriorsList>("StaticData").Configs.ToDictionary(x => x.TypeId, x => x);
        Resources.Load<BossList>("StaticData").Configs.ToDictionary(x => x.Stage, x => x);
    }

    public WarriorConfig GetWarrior(WarriorTypeId typeId) =>
        _warriors.TryGetValue(typeId, out WarriorConfig config)
            ? config : null;

    public BossConfig GetBoss(int stage) =>
        _bosses.TryGetValue(stage, out BossConfig config)
            ? config : null;
}