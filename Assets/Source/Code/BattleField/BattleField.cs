using Source.Code.BattleField.View;
using UnityEngine;

namespace Source.Code.BattleField
{
    public class BattleField : GameWindow
    {
        [SerializeField] private BattleFieldView _view;
        
        private BattleFieldPresenter _presenter;

        private void Awake() 
        {
            var battleService = Provider.Get<BattleFieldService>();
            _presenter = new BattleFieldPresenter(battleService.BattleFieldModel, _view);
        }
    }

    public class BattleFieldPresenter
    {
        private readonly IReadOnlyBattleFieldModel _model;
        private readonly BattleFieldView _view;


        public BattleFieldPresenter(IReadOnlyBattleFieldModel model, BattleFieldView view)
        {
            _model = model;
            _view = view;
        }
    }
}