using System.Collections.Generic;
using Source.Code.IdleNumbers;
using Source.Code.StaticData;
using Source.Code.Warriors;
using UnityEngine;

namespace Source.Code.ModelsAndServices.BattleField
{
    public interface IReadOnlyBattleFieldModel
    {
        IdleNumber BossMaxHp { get; }
        IdleNumber BossCurrentHp { get; }
        Sprite BossSprite { get; }
    }


    public class BattleFieldModel : IReadOnlyBattleFieldModel
    {
        public List<CharacterTypeId> SelectedWarriors = new();
        public List<Warrior> Warriors = new();
        public BossAttackConfig AttackConfig;
        public IdleNumber BossMaxHp { get; set; }
        public IdleNumber BossCurrentHp { get; set; }
        public Sprite BossSprite { get; set; }

    }
}