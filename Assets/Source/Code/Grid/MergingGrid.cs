using Source.Code.Grid.View;
using Source.Code.ModelsAndServices;
using Source.Code.ModelsAndServices.Grid;
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
            var warriorFactory = Provider.Get<IWarriorFactory>();
            _presenter = new MergeGridPresenter(service, warriorFactory, _view);
        }
        
        private void OnDestroy()
        {
            _presenter.CleanUp();
        }
    }
}