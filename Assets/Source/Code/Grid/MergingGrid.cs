using Source.Code.Grid.View;
using Source.Code.Services;
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
            var staticData = Provider.Get<StaticDataService>();
            _presenter = new MergeGridPresenter(service, staticData, _view);
        }

        private void OnDestroy()
        {
            _presenter.CleanUp();
        }
    }
}