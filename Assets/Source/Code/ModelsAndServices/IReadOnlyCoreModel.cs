using Source.Code.ModelsAndServices.Grid;
using Source.Code.ModelsAndServices.Player;

namespace Source.Code.ModelsAndServices
{
    public interface IReadOnlyCoreModel
    {
        PlayerModel Player { get;  }
        GridModel Grid { get; }
    }
}