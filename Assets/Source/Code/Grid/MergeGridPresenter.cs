using System;
using System.Collections.Generic;
using System.Linq;
using Source.Code.Grid.View;
using Source.Code.Models;
using Source.Code.Services;
using Source.Code.StaticData;

namespace Source.Code.Grid
{
    public class MergeGridPresenter
    {
        private readonly MergeGridView _view;
        private readonly MergeGridService _gridService;
        private readonly StaticDataService _staticData;
        private readonly IMergeGridModel _model;

        public MergeGridPresenter(MergeGridService service, StaticDataService staticData, MergeGridView view)
        {
            _gridService = service;
            _staticData = staticData;
            _model = _gridService.GridModel;
            _view = view;

            InitView();

            _view.CellsMergeAttempt += HandleMergeAttempt;
            _view.CreateButtonClicked += OnCreateButtonClicked;
            _view.DragStarted += OnDragStarted;
        }

        private void InitView()
        {
            var selectedWarriors = new List<IWarrior>();
            
            foreach (var warriorTypeId in _gridService.SelectedWarriors)
            {
                var warriorConfig = _staticData.GetWarrior(warriorTypeId);
                
                if(warriorConfig == null)
                    throw new NullReferenceException($"Missing warrior config by {warriorTypeId} type");
                
                var warrior = new Warrior
                {
                    TypeId = warriorConfig.TypeId,
                    Icon = warriorConfig.Sprite,
                    Health = warriorConfig.Health,
                    MaxHealth = warriorConfig.Health,
                    DamagePerSecond = warriorConfig.DamagePerSecond,
                    NormalizedSpeed = warriorConfig.NormalizedSpeed
                };
                
                selectedWarriors.Add(warrior);
            }
            
            _view.Init(_model.GridBoosters, selectedWarriors);
        }

        public void CleanUp()
        {
            _view.CellsMergeAttempt -= HandleMergeAttempt;
            _view.CreateButtonClicked -= OnCreateButtonClicked;
            _view.DragStarted -= OnDragStarted;
        }

        private void HandleMergeAttempt(int hostIndex, int inputIndex)
        {
            if (_gridService.TryMerge(hostIndex, inputIndex, out var booster, out var emptyIndex))
            {
                _view.UpdateGrid(booster, emptyIndex);
                _view.SetActiveBuyButton(_gridService.IsEnableAddNewItem);
            }
            else
            {
                _view.RejectMerge(hostIndex, inputIndex);
            }
        }
        
        private void OnCreateButtonClicked()
        {
            if (_gridService.TryCreateNewBooster(out var booster))
            {
                _view.UpdateGrid(booster);
                _view.SetActiveBuyButton(_gridService.IsEnableAddNewItem);
            }
        }
        
        
        private void OnDragStarted(int index)
        {
            var booster = _model.GridBoosters[index];

            var boostersIndex = _model.GridBoosters.Where(x =>
                x != null &&
                x != booster &&
                x.Level == booster.Level &&
                x.TypeId == booster.TypeId)
                .Select(x => x.Index).ToList();
            
            _view.HighlightMergeTarget(boostersIndex);
        }
    }
}