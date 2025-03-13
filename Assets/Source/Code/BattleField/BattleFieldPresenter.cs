using Source.Code.BattleField.View;
using Source.Code.StaticData;
using UnityEngine;

namespace Source.Code.BattleField
{
    public class BattleFieldPresenter
    {
        private readonly BattleFieldView _view;
        private readonly BattleFieldService _battleFieldService;
        private IReadOnlyBattleFieldModel _model;

        public BattleFieldPresenter(BattleFieldService service, BattleFieldView view)
        {
            _battleFieldService = service;
            _view = view;
            
            _battleFieldService.WarriorAdded += AddWarriorView;
            _battleFieldService.BossHitLine += HitLine;
            _battleFieldService.WarriorSpawned += SpawnWarrior;
            _battleFieldService.ReadyToStart += OnReadyToStart;
            _battleFieldService.StageCompleted += OnStageCompleted;
            _battleFieldService.TickCalculated += OnTickCalculated;
            
            _battleFieldService.PrepareNewStage();
        }
        
        private void OnReadyToStart()
        {
            _model = _battleFieldService.BattleFieldModel;
            _view.Init(_model.BossSprite, _model.BossMaxHp);
            _battleFieldService.Start();
        }
        
        private void OnStageCompleted()
        {
            _view.Clear();
            _battleFieldService.PrepareNewStage();
        }

        private void OnTickCalculated()
        {
            _view.UpdateBossHp(_model.BossCurrentHp, _model.BossMaxHp);
            _view.UpdateWarriors(_model.ReadOnlyWarriors);
        }

        public void CleanUp()
        {
            _battleFieldService.WarriorAdded -= AddWarriorView;
            _battleFieldService.BossHitLine -= HitLine;
            _battleFieldService.WarriorSpawned -= SpawnWarrior;
            _battleFieldService.ReadyToStart -= OnReadyToStart;
            _battleFieldService.StageCompleted -= OnStageCompleted;
            _battleFieldService.TickCalculated -= OnTickCalculated;
        }

        private void SpawnWarrior(IWarrior warrior) => 
            _view.SpawnWarrior(warrior);

        private void HitLine(int index) => 
            _view.HitLine(index);

        private void AddWarriorView(IWarrior warrior) => 
            _view.CreateNewWarrior(warrior);
    }
}