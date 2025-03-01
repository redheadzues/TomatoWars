namespace Source.Code.Models
{
    public class CoreModel
    {
        public GridModel Grid;
        public int Gold;
        public int Gem;
        public int Stage;

        public CoreModel()
        {
            Grid = new GridModel();
        }

    }
}