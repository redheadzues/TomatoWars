using Source.Code.ModelsAndServices;
using Source.Code.ModelsAndServices.BattleField;

namespace Source.Code.BattleField.Buff
{
    public class BattleEffectSystem
    {
        private readonly BattleFieldModel _model;
        private readonly IStaticDataService _staticData;
        
        public BattleEffectSystem(BattleFieldModel model)
        {
            _model = model;
        }

        public void Update()
        {
            
        }
    }
}