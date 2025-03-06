using Source.Code.Models;
using Source.Code.Services;

public interface ICoreModelService : IService
{
    void Init(CoreModel model);
}

