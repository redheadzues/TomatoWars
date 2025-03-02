using System.Collections.Generic;
using UnityEngine;

namespace Source.Code.StaticData
{
    [CreateAssetMenu(fileName = "WarriorList", menuName = "Game Data/Warrior List")]
    public class WarriorsList : ScriptableObject
    {
        [SerializeField] private List<WarriorConfig> _warriors;

        public IReadOnlyList<WarriorConfig> Configs => _warriors;
    }
}