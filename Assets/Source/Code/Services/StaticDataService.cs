using System.Collections.Generic;
using System.Linq;
using Source.Code.Models;
using Source.Code.StaticData;
using UnityEngine;

public class StaticDataService : ICoreModelService
{
    private readonly Dictionary<WarriorType, WarriorConfig> _warriors = new();
    
    public void Init(CoreModel model)
    {
    }

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