using System;
using Source.Code.BattleField.View;
using UnityEngine;

namespace Source.Code.BattleField
{
    public class BattleField : GameWindow
    {
        [SerializeField] private BattleFieldView _view;
        
        private BattleFieldPresenter _presenter;
        private BattleFieldService _battleService;

        private void Start() 
        {
            _battleService = Provider.Get<BattleFieldService>();
            _presenter = new BattleFieldPresenter(_battleService.BattleFieldModel, _view);
            _battleService.Start();
            
        }

        private void Update()
        {
            _view.UpdateWarriors(_battleService.BattleFieldModel.ReadOnlyWarriors);
        }
    }
}