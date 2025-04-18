using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Source.Code.StaticData;
using Source.Code.Warriors;

namespace Source.Code.ModelsAndServices.BattleField
{
    public class BossAttackHandler
    {
        private readonly BattleFieldModel _model;
        private readonly Random _random = new(Guid.NewGuid().GetHashCode());

        private float _cooldownTimer;
        
        public BossAttackHandler(BattleFieldModel model)
        {
            _model = model;
            _cooldownTimer = _model.AttackConfig.Cooldown;
        }

        public void Update(Action<float, float> bossAttacked)
        {
            _cooldownTimer -= StaticConfig.TICK_INTERVAL;
            
            if(_cooldownTimer > 0)
                return;
            
            var attackCenterPoint = new Vector2((float)_random.NextDouble(), (float)_random.NextDouble());
            var attackWidth = _model.AttackConfig.NormalizedWidth;
            
            
            var warriorForAttack = DefineWarriorsForAttack(_model.AttackConfig.TypeId, attackCenterPoint);

            var damage = _model.AttackConfig.DamagePerSecond * _model.AttackConfig.Cooldown;
            
            foreach (var warrior in warriorForAttack)
            {
                warrior.TakeDamage(damage);
            }
            
            _cooldownTimer += _model.AttackConfig.Cooldown;
            bossAttacked?.Invoke(attackCenterPoint.X, attackWidth);
        }

        private List<Warrior> DefineWarriorsForAttack(BossAttackTypeId typeId, Vector2 attackCenterPoint)
        {
            var attackWidth = _model.AttackConfig.NormalizedWidth;

            return DefineWarriorsForLineAttack(attackCenterPoint);

        }

        private List<Warrior> DefineWarriorsForLineAttack(Vector2 attackCenterPoint)
        {
            var attackWidth = _model.AttackConfig.NormalizedWidth;
            
            var warriorForAttack = _model.Warriors
                .Where(x => 
                    x.NormalizePosition.X > attackCenterPoint.X - attackWidth/2
                    &&
                    x.NormalizePosition.X < attackCenterPoint.X + attackWidth/2)
                .ToList();

            return warriorForAttack;
        }
        
    }
}