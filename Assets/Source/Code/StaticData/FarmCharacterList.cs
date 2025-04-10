using System.Collections.Generic;
using UnityEngine;

namespace Source.Code.StaticData
{
    [CreateAssetMenu(fileName = "FarmCharactersList", menuName = "Game Data/Farm Characters List")]
    public class FarmCharacterList : ScriptableObject
    {
        [SerializeField] private List<FarmCharacterConfig> _farmCharacters;
        
        public IReadOnlyList<FarmCharacterConfig> Configs => _farmCharacters;
    }
}