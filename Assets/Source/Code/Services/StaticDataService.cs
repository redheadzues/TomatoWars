using System.Collections.Generic;
using System.Linq;
using Source.Code.Services;
using Source.Code.StaticData;
using UnityEngine;

public class StaticDataService : Service
{
    private readonly Dictionary<WarriorType, WarriorConfig> _warriors = new();
    
    public void LoadData()
    {
        var warriors = Resources.Load<WarriorsList>("StaticData");
        warriors.Configs.ToDictionary(x => x.Type, x => x);
    }

    public WarriorConfig GetWarrior(WarriorType type) =>
        _warriors.TryGetValue(type, out WarriorConfig config)
            ? 
            config : null;
}