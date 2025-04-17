using Source.Code.IdleNumbers;
using Source.Code.ModelsAndServices.Farm;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.Farm
{
    public class FarmPresenter : ICleanable
    {
        private readonly IFarmService _farmService;
        private readonly FarmView _view;
        
        public FarmPresenter(IFarmService farmService, FarmView view)
        {
            _farmService = farmService;
            _view = view;
            _view.Init(_farmService.FarmCharacters);
            _view.UpgradeRequested += OnUpgradeRequested;
            _farmService.BalanceChanged += OnBalanceChanged;
        }

        public void CleanUp() => 
            _view.UpgradeRequested -= OnUpgradeRequested;
        
        private void OnBalanceChanged(IdleNumber value) => 
            _view.UpdateAvailabilityButtons(value);

        private void OnUpgradeRequested(CharacterTypeId typeId)
        {
            var isUpgraded = _farmService.TryUpgradeCharacter(typeId);
            
            if (isUpgraded)
            {
                _view.UpdateCharacterView(typeId);
            }
        }
    }
}