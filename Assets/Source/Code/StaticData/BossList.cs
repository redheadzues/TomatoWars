using System.Collections.Generic;
using UnityEngine;

namespace Source.Code.StaticData
{
    [CreateAssetMenu(fileName = "BossList", menuName = "Game Data/Boss list")]
    public class BossList : ScriptableObject
    {
        [SerializeField] private List<BossConfig> _bosses;
        
        public IReadOnlyList<BossConfig> Configs => _bosses;
    }
}