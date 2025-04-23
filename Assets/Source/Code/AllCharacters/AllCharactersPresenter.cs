using Source.Code.AllCharacters.View;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;

namespace Source.Code.AllCharacters
{
    public class AllCharactersPresenter : ICleanable
    {
        private readonly IPlayerService _playerService;
        private readonly AllCharactersView _view;

        public AllCharactersPresenter(IPlayerService playerService, AllCharactersView view)
        {
            _playerService = playerService;
            _view = view;
            InitView();
        }
        public void CleanUp()
        {
            _view.UpgradeRequired -= OnUpgradeRequired;
            _view.RequiredBuyShardsPack -= OnRequiredBuyShardsPack;
        }

        private void InitView()
        {
            _view.UpgradeRequired += OnUpgradeRequired;
            _view.RequiredBuyShardsPack += OnRequiredBuyShardsPack;
            var allCharacters = _playerService.GetAllOwnedWarriors();
            _view.Init(allCharacters);
        }

        private void OnUpgradeRequired(CharacterTypeId typeId)
        {
            if (_playerService.TryLevelUpWarrior(typeId))
            {
                _view.UpdateCharacterView(typeId);
            }
        }
        
        
        private void OnRequiredBuyShardsPack()
        {
            
        }

    }
}