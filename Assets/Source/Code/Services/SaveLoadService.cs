using Source.Code.Models;

public class SaveLoadService : ICoreModelService
{
    private CoreModel _model;
    
    public void Init(CoreModel model)
    {
        _model = model;
    }
    
    public void Load(){}
    
    public void Save(){}
    
}