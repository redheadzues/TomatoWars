using Source.Code.Grid.View;
using Source.Code.Services;
using UnityEngine;

namespace Source.Code.Grid
{
    public class MergingGrid : GameWindow
    {
        [SerializeField] private MergeGridView _view;

        private MergeGridPresenter _presenter;

        protected override void OnProviderInitialized()
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