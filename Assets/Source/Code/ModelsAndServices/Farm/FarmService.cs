using Source.Code.ModelsAndServices.Player;

namespace Source.Code.ModelsAndServices.Farm
{
    public interface IFarmService : IService
    {
        
    }
    
    public class FarmService : IFarmService
    {
        private readonly IPlayerService _playerService;
        private readonly IStaticDataService _staticDataService;
        private readonly FarmModel _model;
        
        
    }
}