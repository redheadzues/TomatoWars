using Source.Code.BattleField.View;
using UnityEngine;

namespace Source.Code.BattleField
{
    public class BattleField : GameWindow
    {
        [SerializeField] private BattleFieldView _view;
        
        private BattleFieldPresenter _presenter;
        private IBattleFieldService _battleService;

        protected override void OnProviderInitialized()
        {
            _battleService = Provider.Get<IBattleFieldService>();
            _presenter = new BattleFieldPresenter(_battleService, _view);
        }
        
        private void OnDestroy()
        {
            _presenter.CleanUp();
        }
    }
}