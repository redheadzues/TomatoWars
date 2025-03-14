using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.Grid.View
{
    public class MergeGridView : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private Button _createNewBoosterButton;
        [SerializeField] private CellView _cellPrefab;
        [SerializeField] private GridLayoutGroup _grid;
        
        private List<CellView> _cellViews;
        private MergeCollisionHandler _mergeCollisionHandler;

        public event Action<int, int> DragCompleted;
        public event Action CreateButtonClicked;

        private void Awake()
        {
            foreach (Transform child in _grid.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnEnable()
        {
            _createNewBoosterButton.onClick.AddListener(OnCreateButtonClicked);
        }

        private void OnDisable()
        {
            _createNewBoosterButton.onClick.RemoveListener(OnCreateButtonClicked);
        }

        public void Init(IReadOnlyList<GridBooster> boosters)
        {
            _mergeCollisionHandler = new MergeCollisionHandler(this, this);
            
            _cellViews = new(30);
            
            for (var i = 0; i < boosters.Count; i++)
            {
                var cell = Instantiate(_cellPrefab, _grid.transform);
                cell.SetBooster(boosters[i]);
                cell.Init(i);
                _cellViews.Add(cell);
                _mergeCollisionHandler.AddCellView(cell);
            }
        }

        public void SetActiveBuyButton(bool isActive)
        {
            _createNewBoosterButton.interactable = isActive;
        }

        public void UpdateGrid(GridBooster booster, int emptyIndex)
        {
            UpdateGrid(booster);
            _cellViews[emptyIndex].SetBooster(null);
        }

        public void UpdateGrid(GridBooster booster)
        {
            _cellViews[booster.Index].SetBooster(booster);
        }

        public void FailDrag(int firstIndex, int secondIndex)
        {
            _cellViews[firstIndex].AnimateFail();
            _cellViews[secondIndex].ReturnPosition();
        }
        
        private void OnCreateButtonClicked()
        {
            CreateButtonClicked?.Invoke();
        }
        
        

    }
}