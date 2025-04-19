using Source.Code.ModelsAndServices.Player;
using UnityEngine;

namespace Source.Code.CurrencyDisplayWindow
{
    public class CurrencyDisplayWindow : GameWindow
    {
        [SerializeField] private CurrencyDisplayView _view;

        private CurrencyDisplayPresenter _presenter;

        private void OnDestroy() => 
            _presenter.CleanUp();

        protected override void OnProviderInitialized()
        {
            var playerService = Provider.Get<IPlayerService>();
            _presenter = new CurrencyDisplayPresenter(playerService, _view);
        }
    }
}
