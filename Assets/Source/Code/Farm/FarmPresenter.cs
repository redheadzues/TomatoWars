using Source.Code.ModelsAndServices.Farm;

namespace Source.Code.Farm
{
    public class FarmPresenter
    {
        private readonly IFarmService _farmService;
        private readonly FarmView _view;
        
        public FarmPresenter(IFarmService farmService, FarmView view)
        {
            _farmService = farmService;
            _view = view;
        }
    }
}