using System.Collections.Generic;
using UnityEngine;

namespace Source.Code.StaticData
{
    [CreateAssetMenu(fileName = "BoosterList", menuName = "Game Data/Booster List")]
    public class BoosterList : ScriptableObject
    {
        [SerializeField] private List<BoosterConfig> _boosters;
        
        public IReadOnlyList<BoosterConfig> Configs => _boosters;
    }
}