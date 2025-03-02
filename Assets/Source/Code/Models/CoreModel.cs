namespace Source.Code.Models
{
    public class CoreModel
    {
        public GridModel Grid;
        public PlayerModel Player;


        public CoreModel()
        {
            Grid = new GridModel();
            Player = new PlayerModel();
        }

    }
}