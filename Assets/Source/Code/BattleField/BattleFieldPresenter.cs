using Source.Code.BattleField.View;
using Source.Code.ModelsAndServices.BattleField;
using Source.Code.StaticData;
using Source.Code.Warriors;
using UnityEngine;

namespace Source.Code.BattleField
{
    public class BattleFieldPresenter
    {
        private readonly BattleFieldView _view;
        private readonly IBattleFieldService _battleFieldService;

        public BattleFieldPresenter(IBattleFieldService service, BattleFieldView view)
        {
            _battleFieldService = service;
            _view = view;
            
            _battleFieldService.WarriorAdded += AddWarriorView;
            _battleFieldService.BossAttacked += OnBossAttacked;
            _battleFieldService.ReadyToStart += OnReadyToStart;
            _battleFieldService.StageCompleted += OnStageCompleted;
            _battleFieldService.TickCalculated += OnTickCalculated;
            
            _battleFieldService.PrepareNewStage();
        }
        
        public void CleanUp()
        {
            _battleFieldService.WarriorAdded -= AddWarriorView;
            _battleFieldService.BossAttacked -= OnBossAttacked;
            _battleFieldService.ReadyToStart -= OnReadyToStart;
            _battleFieldService.StageCompleted -= OnStageCompleted;
            _battleFieldService.TickCalculated -= OnTickCalculated;
        }
        
        private void OnReadyToStart()
        {
           
            _view.Init(_battleFieldService.Model.BossSprite, _battleFieldService.Model.BossMaxHp);
            _battleFieldService.Start();
        }
        
        private void OnStageCompleted()
        {
            _view.Clear();
            _battleFieldService.PrepareNewStage();
        }

        private void OnTickCalculated()
        {
            _view.UpdateBossHp(_battleFieldService.Model.BossCurrentHp, _battleFieldService.Model.BossMaxHp);
            _view.UpdateWarriors();
        }

        private void OnBossAttacked(float centerAttack, float widthAttack) => 
            _view.ShowBossAttack(centerAttack, widthAttack);

        private void AddWarriorView(IWarrior warrior) => 
            _view.CreateNewWarrior(warrior);
    }
}