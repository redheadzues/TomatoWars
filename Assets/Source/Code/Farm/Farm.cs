using Source.Code.Farm.View;
using Source.Code.ModelsAndServices.Farm;
using UnityEngine;

namespace Source.Code.Farm
{
    public class Farm : GameWindow
    {
        [SerializeField] private FarmView _view;

        private FarmPresenter _presenter;
        private IFarmService _farmService;
        
        protected override void OnProviderInitialized()
        {
            _farmService = Provider.Get<IFarmService>();
            _presenter = new FarmPresenter(_farmService, _view);
        }
    }
}