using Source.Code.Models.Player;

namespace Source.Code.Models
{
    public interface IReadOnlyCoreModel
    {
        PlayerModel Player { get;  }
        GridModel Grid { get; }
    }
}