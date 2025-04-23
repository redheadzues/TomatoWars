using Source.Code.AllCharacters.View;
using Source.Code.ModelsAndServices.Player;
using Source.Code.StaticData;

namespace Source.Code.AllCharacters
{
    public class AllCharactersPresenter
    {
        private readonly IPlayerService _playerService;
        private readonly AllCharactersView _view;

        public AllCharactersPresenter(IPlayerService playerService, AllCharactersView view)
        {
            _playerService = playerService;
            _view = view;
            InitView();
        }

        private void InitView()
        {
            _view.UpgradeRequired += OnUpgradeRequired;
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
    }
}