using System.Collections;
using System.Collections.Generic;
using Source.Code.Grid.View;
using Source.Code.Models;
using UnityEngine;

namespace Source.Code.Grid
{
    public class MergingGrid : GameWindow
    {
        [SerializeField] private MergeGridView _view;

        private MergeGridPresenter _presenter;
        
        private void Start()
        {
            var service = Provider.Get<MergeGridService>();
            _presenter = new MergeGridPresenter(service, _view);
        }

        private void OnDestroy()
        {
            _presenter.CleanUp();
        }
    }
    
    public class MergeCollisionHandler
    {
        private readonly MergeGridView _view;
        private readonly Dictionary<Collider2D, CellView> _colliderToCellView = new();
        private readonly HashSet<Collider2D> _triggeredColliders = new();
        private readonly ICoroutineRunner _coroutineRunner;
        
        private CellView _draggingCellView;
        private CellView _bestOverlapView;
        private Coroutine _overlapCoroutine;
        
        public MergeCollisionHandler(MergeGridView view, ICoroutineRunner coroutineRunner)
        {
            _view = view;
            _coroutineRunner = coroutineRunner;
        }

        public void AddCellView(CellView view)
        {
            _colliderToCellView.Add(view.Collider, view);
            view.Draggable.DragStarted += OnDragStarted;
            view.Draggable.DragEnded += OnDragEnded;
        }

        public void CleanUp()
        {
            foreach (var view in _colliderToCellView.Values)
            {
                view.Draggable.DragStarted -= OnDragStarted;
                view.Draggable.DragEnded -= OnDragEnded;
            }

            if (_draggingCellView != null)
            {
                _draggingCellView.Draggable.TriggerEnter -= OnDraggableTriggerEnter;
                _draggingCellView.Draggable.TriggerExit -= OnDraggableTriggerExit;
            }
            
            StopCoroutine();
        }

        private void OnDragStarted(BoosterIconDraggable draggable, CellView cellView)
        {
            draggable.TriggerEnter += OnDraggableTriggerEnter;
            draggable.TriggerExit += OnDraggableTriggerExit;
            _overlapCoroutine = _coroutineRunner.StartCoroutine(OverlapCoroutine());
            _draggingCellView = cellView;
        }
        
        private void OnDragEnded(BoosterIconDraggable draggable)
        {
            draggable.TriggerEnter -= OnDraggableTriggerEnter;
            draggable.TriggerExit -= OnDraggableTriggerExit;
            
            _draggingCellView = null;
            _triggeredColliders.Clear();
            _bestOverlapView?.HighlightSetActive(false);
            StopCoroutine();
        }
        
        private void OnDraggableTriggerEnter(Collider2D collider)
        {
            _triggeredColliders.Add(collider);
        }
        
        private void OnDraggableTriggerExit(Collider2D collider)
        {
            _triggeredColliders.Remove(collider);
        }
        
        private void GetBestOverlap()
        {
            if (_triggeredColliders.Count == 0)
            {
                _bestOverlapView?.HighlightSetActive(false);
                _bestOverlapView = null;
                return;
            }

            Collider2D bestCollider = null;
            float maxOverlapArea = 0f;

            foreach (var collider in _triggeredColliders)
            {
                float overlapArea = GetOverlapArea(_draggingCellView.Draggable.Collider.bounds, collider.bounds);
                if (overlapArea > maxOverlapArea)
                {
                    maxOverlapArea = overlapArea;
                    bestCollider = collider;
                }
            }

            if (bestCollider != null && _bestOverlapView != _colliderToCellView[bestCollider])
            {
                _bestOverlapView?.HighlightSetActive(false);
                _bestOverlapView = _colliderToCellView[bestCollider];
                _bestOverlapView.HighlightSetActive(true);
            }
        }

        private float GetOverlapArea(Bounds a, Bounds b)
        {
            float xOverlap = Mathf.Max(0, Mathf.Min(a.max.x, b.max.x) - Mathf.Max(a.min.x, b.min.x));
            float yOverlap = Mathf.Max(0, Mathf.Min(a.max.y, b.max.y) - Mathf.Max(a.min.y, b.min.y));
            return xOverlap * yOverlap;
        }

        private IEnumerator OverlapCoroutine()
        {
            while (true)
            {
                GetBestOverlap();
                yield return null;
            }
        }

        private void StopCoroutine()
        {
            if(_overlapCoroutine != null)
                _coroutineRunner.StopCoroutine(_overlapCoroutine);
        }
    }

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
            _view.DragCompleted += OnDragCompleted;
            _view.CreateButtonClicked += OnCreateButtonClicked;
        }

        public void CleanUp()
        {
            _view.DragCompleted -= OnDragCompleted;
            _view.CreateButtonClicked -= OnCreateButtonClicked;
        }
        
        private void OnDragCompleted(int firstIndex, int secondIndex)
        {
            if (_gridService.TryMerge(firstIndex, secondIndex, out var booster, out var emptyIndex))
            {
                _view.UpdateGrid(booster, emptyIndex);
            }
            else
            {
                _view.FailDrag(firstIndex, secondIndex);
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
    }
}