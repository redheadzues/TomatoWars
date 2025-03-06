namespace Source.Code.Services
{
    public interface ILocalModelService<T> : IService where T : class
    {
        void SetLocalModel(T model);
    }
}