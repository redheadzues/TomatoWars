using Source.Code.Grid.View;
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
}