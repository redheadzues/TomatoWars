using System;
using System.Linq;
using Source.Code.Grid.View;
using Source.Code.Models;
using UnityEngine;

namespace Source.Code.Grid
{
    public class MergeGridPresenter
    {
        private readonly MergeGridView _view;
        private readonly MergeGridService _gridService;
        private readonly IMergeGridModel _model;

        public MergeGridPresenter(MergeGridService service, MergeGridView view)
        {
            _gridService = service;
            _model = _gridService.GridModel;
            _view = view;

            _view.Init(_model.GridBoosters);
            
            _view.CellsMergeAttempt += HandleMergeAttempt;
            _view.CreateButtonClicked += OnCreateButtonClicked;
            _view.DragStarted += OnDragStarted;
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
            var booster = _model.GridBoosters.FirstOrDefault(x => x.Index == index);

            if (booster == null)
            {
                throw new NullReferenceException($"booster is null index is {index}");
            }

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