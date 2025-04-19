using System;
using Source.Code.IdleNumbers;
using Source.Code.ModelsAndServices.Player;

namespace Source.Code.CurrencyDisplayWindow
{
    public class CurrencyDisplayPresenter : ICleanable
    {
        private readonly IPlayerService _playerService;
        private readonly CurrencyDisplayView _view;
       
        public void CleanUp() => 
            _playerService.BalanceChanged -= OnBalanceChanged;

        public CurrencyDisplayPresenter(IPlayerService playerService, CurrencyDisplayView view)
        {
            _playerService = playerService;
            _view = view;

            _playerService.BalanceChanged += OnBalanceChanged;

            InitView();
        }

        private void InitView()
        {
            foreach (CurrencyTypeId typeId in Enum.GetValues(typeof(CurrencyTypeId)))
            {
                var value = _playerService.GetCurrencyBalance(typeId);
                _view.SetCurrencyValue(typeId, value);
            }
        }

        private void OnBalanceChanged(CurrencyTypeId typeId, IdleNumber value) => 
            _view.SetCurrencyValue(typeId, value);
    }
}