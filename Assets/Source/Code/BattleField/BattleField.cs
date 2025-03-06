using System.Collections.Generic;
using Source.Code.Models;
using Source.Code.Services;
using Source.Code.StaticData;
using Unity.VisualScripting;
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
            var battleModel = new BattleFieldModel();
            battleService.SetLocalModel(battleModel);
            _presenter = new BattleFieldPresenter(battleModel, battleService, _view);
        }
    }

    public class BattleFieldPresenter
    {
        private readonly BattleFieldModel _model;
        private readonly BattleFieldService _battleService;
        private readonly BattleFieldView _view;


        public BattleFieldPresenter(BattleFieldModel model, BattleFieldService service, BattleFieldView view)
        {
            _model = model;
            _battleService = service;
            _view = view;
        }
    }

    public class BattleFieldService : ICoreModelService, ILocalModelService<BattleFieldModel>
    {
        private CoreModel _coreModel;
        private BattleFieldModel _battleModel;
        
        public void Init(CoreModel model)
        {
            _coreModel = model;
        }

        public void SetLocalModel(BattleFieldModel model)
        {
            _battleModel = model;
        }

        public void StartGame()
        {
            _battleModel.SelectedWarriors = _coreModel.Player.SelectedWarrior;
        }
    }

    public class BattleFieldModel
    {
        public List<WarriorType> SelectedWarriors;
    }
}