using System;
using System.Collections.Generic;
using System.Linq;
using Source.Code.IdleNumbers;
using Source.Code.ModelsAndServices.Farm;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.Farm.View
{
    public class FarmView : MonoBehaviour
    {
        [SerializeField] private FarmCharacterView _characterPrefab;
        [SerializeField] private Transform _container;

        private readonly List<FarmCharacterView> _views = new();

        public event Action<CharacterTypeId> UpgradeRequested;

        private void OnEnable() => 
            _views.ForEach(x => x.UpgradeRequested += OnUpgradeRequested);

        private void OnDisable() => 
            _views.ForEach(x => x.UpgradeRequested -= OnUpgradeRequested);

        public void Init(IReadOnlyList<IFarmCharacter> characters)
        {
            var sortedCharacters = characters.OrderBy(x => (int)x.TypeId).ToList();
            
            foreach (var farmCharacter in sortedCharacters)
            {
                var view = Instantiate(_characterPrefab, _container);
                view.Init(farmCharacter);
                view.UpgradeRequested += OnUpgradeRequested;
                _views.Add(view);
            }
        }

        public void UpdateAvailabilityButtons(IdleNumber value) => 
            _views.ForEach(x => x.UpdateAvailabilityButton(value));

        public void UpdateCharacterView(CharacterTypeId typeId)
        {
            var view = _views.FirstOrDefault(x => x.TypeId == typeId);
            
            view?.UpdateTextData();
        }

        private void OnUpgradeRequested(CharacterTypeId typeId) => 
            UpgradeRequested?.Invoke(typeId);

    }
}