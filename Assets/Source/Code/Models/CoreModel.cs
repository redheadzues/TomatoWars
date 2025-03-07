using Source.Code.BattleField;

namespace Source.Code.Models
{
    public interface IReadOnlyCoreModel
    {
        PlayerModel Player { get;  }
    }
    

    public class CoreModel : IReadOnlyCoreModel
    {
        public PlayerModel Player { get; set; }
    }
    
}