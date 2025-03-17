using System;
using System.Collections.Generic;
using System.Linq;
using Source.Code.Grid.View;
using Source.Code.ModelsAndServices;
using Source.Code.ModelsAndServices.Grid;
using Source.Code.StaticData;

namespace Source.Code.Grid
{
    public class MergeGridPresenter
    {
        private readonly MergeGridView _view;
        private readonly IMergeGridService _gridService;
        private readonly IStaticDataService _staticData;

        public MergeGridPresenter(IMergeGridService service, IStaticDataService staticData, MergeGridView view)
        {
            _gridService = service;
            _staticData = staticData;
            _view = view;

            _view.CellsMergeAttempt += HandleMergeAttempt;
            _view.CreateButtonClicked += OnCreateButtonClicked;
            _view.DragStarted += OnDragStarted;

            InitView();
        }

        private void InitView()
        {
            var selectedWarriors = new List<IWarrior>();
            
            foreach (var warriorTypeId in _gridService.SelectedWarriors)
            {
                var warriorConfig = _staticData.GetWarrior(warriorTypeId);
                
                if(warriorConfig == null)
                    throw new NullReferenceException($"Missing warrior config by {warriorTypeId} type");

                var warrior = new Warrior(warriorConfig);
                
                selectedWarriors.Add(warrior);
            }
            
            _view.Init(_gridService.GridModel.GridBoosters, selectedWarriors);
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