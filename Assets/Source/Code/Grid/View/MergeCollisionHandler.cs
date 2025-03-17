using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Code.Grid.View
{
    public class MergeCollisionHandler
    {
        private readonly MergeGridView _view;
        private readonly Dictionary<Collider2D, CellView> _colliderToCellView = new();
        private readonly Dictionary<Collider2D, WarriorView> _colliderToWarriorView = new();
        private readonly HashSet<Collider2D> _triggeredColliders = new();
        private readonly ICoroutineRunner _coroutineRunner;
        
        private CellView _draggingCellView;
        private Collider2D _bestOverlap;
        private Coroutine _overlapCoroutine;
        
        public event Action<IHighlightElement> ElementOverlaps;
        
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

        public void AddWarriorView(WarriorView view)
        {
            _colliderToWarriorView.Add(view.Collider, view);
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
                _draggingCellView = null;
            }
            
            StopCoroutine();
        }

        private void OnDragStarted(BoosterIconDraggable draggable)
        {
            draggable.TriggerEnter += OnDraggableTriggerEnter;
            draggable.TriggerExit += OnDraggableTriggerExit;
            _overlapCoroutine = _coroutineRunner.StartCoroutine(OverlapCoroutine());
            _draggingCellView = draggable.Parent;
        }
        
        private void OnDragEnded(BoosterIconDraggable draggable)
        {
            if(_bestOverlap != null)
                _view.MergeAttempt(GetElementByCollider(_bestOverlap), _draggingCellView.Index);
            else
                draggable.ReturnPosition();
            
            draggable.TriggerEnter -= OnDraggableTriggerEnter;
            draggable.TriggerExit -= OnDraggableTriggerExit;
            
            _draggingCellView = null;
            _triggeredColliders.Clear();
            ElementOverlaps?.Invoke(null);
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
                ElementOverlaps?.Invoke(null);
                _bestOverlap = null;
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

            if (bestCollider != null && _bestOverlap != bestCollider)
            {
                _bestOverlap = bestCollider;
                var highlight = GetElementByCollider(_bestOverlap);
                ElementOverlaps?.Invoke(highlight);
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

        private IHighlightElement GetElementByCollider(Collider2D collider)
        {
            return _colliderToCellView.GetValueOrDefault(collider) ??
                   _colliderToWarriorView.GetValueOrDefault(collider) as IHighlightElement;
        }
    }
}