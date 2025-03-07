using System.Collections.Generic;
using Source.Code.StaticData;

namespace Source.Code.BattleField
{
    public interface IReadOnlyBattleFieldModel
    {
        IReadOnlyList<WarriorType> SelectedWarriors { get; }
        public int BossHp  { get; }
        public int BossCurrentHp { get; }
    }
    
    
    public class BattleFieldModel : IReadOnlyBattleFieldModel
    {
        public List<WarriorType> SelectedWarriors { get; set; }
        IReadOnlyList<WarriorType> IReadOnlyBattleFieldModel.SelectedWarriors => SelectedWarriors;

        public List<Warrior> Warriors;
        
        public int BossHp { get; set; }
        public int BossCurrentHp { get; set; }
        
        
        

    }
}