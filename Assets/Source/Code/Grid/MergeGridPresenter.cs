using System;
using System.Collections.Generic;
using System.Linq;
using Source.Code.BattleField;
using Source.Code.Grid.View;
using Source.Code.ModelsAndServices;
using Source.Code.ModelsAndServices.Grid;
using Source.Code.StaticData;
using Source.Code.Warriors;

namespace Source.Code.Grid
{
    public class MergeGridPresenter
    {
        private readonly MergeGridView _view;
        private readonly IMergeGridService _gridService;
        private readonly IWarriorFactory _warriorFactory;

        public MergeGridPresenter(IMergeGridService service, IWarriorFactory warriorFactory, MergeGridView view)
        {
            _gridService = service;
            _warriorFactory = warriorFactory;
            _view = view;

            _view.BoostersMergeAttempt += HandleMergeAttempt;
            _view.CreateButtonClicked += OnCreateButtonClicked;
            _view.DragStarted += OnDragStarted;
            _view.WarriorBoosterMergeAttempt += HandleMergeAttempt;

            InitView();
        }

        private void InitView()
        {
            var selectedWarriors = new List<IWarrior>();
            
            foreach (var warriorTypeId in _gridService.SelectedWarriors)
            {
                var warrior = _warriorFactory.CreateWarrior(warriorTypeId);
                
                selectedWarriors.Add(warrior);
            }
            
            _view.Init(_gridService.GridModel.GridBoosters, selectedWarriors);
        }

        public void CleanUp()
        {
            _view.BoostersMergeAttempt -= HandleMergeAttempt;
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
        
        private void HandleMergeAttempt(CharacterTypeId characterType, int boosterIndex)
        {
            if (_gridService.TryMerge(characterType, boosterIndex, out var booster))
            {
                _view.UpdateGrid(booster);
                _view.SetActiveBuyButton(_gridService.IsEnableAddNewItem);
                var updatedWarrior = _warriorFactory.CreateWarrior(characterType);
                _view.UpdateWarrior(updatedWarrior);
            }
            else
            {
                _view.RejectMerge(boosterIndex);
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
            var booster = _gridService.GridModel.GridBoosters[index];

            var boostersIndex = _gridService.GridModel.GridBoosters.Where(x =>
                x != null &&
                x != booster &&
                x.Level == booster.Level &&
                x.TypeId == booster.TypeId)
                .Select(x => x.Index).ToList();
            
            _view.HighlightMergeTarget(boostersIndex);
        }
    }
}