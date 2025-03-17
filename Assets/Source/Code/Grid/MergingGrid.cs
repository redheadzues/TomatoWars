using Source.Code.Grid.View;
using Source.Code.ModelsAndServices.Grid;
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
            var service = Provider.Get<IMergeGridService>();
            var staticData = Provider.Get<IStaticDataService>();
            _presenter = new MergeGridPresenter(service, staticData, _view);
        }
        
        private void OnDestroy()
        {
            _presenter.CleanUp();
        }


    }
}