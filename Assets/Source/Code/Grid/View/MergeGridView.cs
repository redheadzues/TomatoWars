using System;
using System.Collections.Generic;
using System.Linq;
using Source.Code.BattleField;
using Source.Code.StaticData;
using Source.Code.Warriors;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.Grid.View
{
    public class MergeGridView : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private Button _createNewBoosterButton;
        [SerializeField] private CellView _cellPrefab;
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private Transform _warriorContainer;
        [SerializeField] private WarriorView _warriorPrefab;
        
        private readonly List<CellView> _cellViews = new(StaticConfig.BOOSTER_LIMIT);
        private readonly List<WarriorView> _selectedWarriors = new();
        
        private MergeCollisionHandler _mergeCollisionHandler;
        private List<int> _mergeTargetIndexes;
        private IHighlightElement _lastHighlight;
        
        public event Action CreateButtonClicked;
        public event Action<int, int> BoostersMergeAttempt;
        public event Action<CharacterTypeId, int> WarriorBoosterMergeAttempt;
        public event Action<int> DragStarted;

        private void OnEnable()
        {
            _createNewBoosterButton.onClick.AddListener(OnCreateButtonClicked);
            _cellViews.ForEach(x => x.Draggable.DragStarted += OnDragStarted);
            _cellViews.ForEach(x => x.Draggable.DragEnded += OnDragEnded);
        }

        private void OnDisable()
        {
            _createNewBoosterButton.onClick.RemoveListener(OnCreateButtonClicked);
            _cellViews.ForEach(x => x.Draggable.DragStarted -= OnDragStarted);
            _cellViews.ForEach(x => x.Draggable.DragEnded -= OnDragEnded);
        }

        private void OnDestroy()
        {
            if(_mergeCollisionHandler != null)
                _mergeCollisionHandler.ElementOverlaps -= OnElementOverlaps;
            
            _mergeCollisionHandler?.CleanUp();
        }

        public void Init(IReadOnlyList<GridBooster> boosters, List<IWarrior> selectedWarriors)
        {
            ClearGrid();
            
            _mergeCollisionHandler = new MergeCollisionHandler(this, this);
            _mergeCollisionHandler.ElementOverlaps += OnElementOverlaps;
           
            for (var i = 0; i < boosters.Count; i++)
            {
                var cell = Instantiate(_cellPrefab, _grid.transform);
                cell.Init(i);
                cell.SetBooster(boosters[i]);
                _cellViews.Add(cell);
                cell.Draggable.DragStarted += OnDragStarted;
                cell.Draggable.DragEnded += OnDragEnded;
                _mergeCollisionHandler.AddCellView(cell);
            }
            
            foreach (var warrior in selectedWarriors)
            {
                var view = Instantiate(_warriorPrefab, _warriorContainer);
                view.SetWarrior(warrior);
                _selectedWarriors.Add(view);
                _mergeCollisionHandler.AddWarriorView(view);
            }
        }

        public void UpdateWarrior(IWarrior warrior)
        {
            var view = _selectedWarriors.FirstOrDefault(x => x.TypeId == warrior.TypeId);

            if (view == null)
            {
                Debug.LogWarning($"Cant find view for warrior type {warrior.TypeId} for update");
                return;
            }
            
            view.SetWarrior(warrior);
        }

        public void SetActiveBuyButton(bool isActive)
        {
            _createNewBoosterButton.interactable = isActive;
        }

        public void UpdateGrid(GridBooster booster, int emptyIndex)
        {
            UpdateGrid(booster);
            _cellViews[emptyIndex].SetBooster(new GridBooster(emptyIndex));
        }

        public void UpdateGrid(GridBooster booster)
        {
            _cellViews[booster.Index].SetBooster(booster);
        }

        public void RejectMerge(int hostIndex, int inputIndex)
        {
            _cellViews[hostIndex].AnimateFail();
            _cellViews[inputIndex].ReturnPosition();
        }

        public void RejectMerge(int index)
        {
            _cellViews[index].ReturnPosition();
        }

        public void HighlightMergeTarget(List<int> targetIndexes)
        {
            _mergeTargetIndexes = targetIndexes;
            
            _mergeTargetIndexes.ForEach(i => _cellViews[i].HighlightAsTarget(true));
        }

        public void MergeAttempt(IHighlightElement hostElement, int inputIndex)
        {
            if (hostElement is CellView cellView)
            {
                BoostersMergeAttempt?.Invoke(cellView.Index, inputIndex);
            }
            else if (hostElement is WarriorView warriorView)
            {
                WarriorBoosterMergeAttempt?.Invoke(warriorView.TypeId, inputIndex);
            }
        }
        
        private void ClearGrid()
        {
            foreach (Transform child in _grid.transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        
        private void OnElementOverlaps(IHighlightElement element)
        {
            if (element == null)
            {
                _lastHighlight?.Highlight(false);
                _lastHighlight = null;
            }
            else
            {
                _lastHighlight?.Highlight(false);
                _lastHighlight = element;
                _lastHighlight.Highlight(true);
            }
        }

        private void OnDragEnded(BoosterIconDraggable draggable) => 
            _mergeTargetIndexes?.ForEach(i => _cellViews[i].HighlightAsTarget(false));

        private void OnCreateButtonClicked() => 
            CreateButtonClicked?.Invoke();

        private void OnDragStarted(BoosterIconDraggable draggable) => 
            DragStarted?.Invoke(draggable.Parent.Index);
    }
}