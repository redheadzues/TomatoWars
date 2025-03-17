namespace Source.Code.ModelsAndServices.Player
{
    public interface IReadOnlyPlayerModel
    {
        int Gold { get;}
        int Gem { get;}
        int Stage { get;}
    }
}