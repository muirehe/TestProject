using System;
using Gameplay.Combat;
using Gameplay.Player;
using MessagePipe;
using Meta;
using VContainer.Unity;

namespace Gameplay.Statistics
{
    public class BattleStatisticsService : IInitializable, IDisposable
    {
        private IDisposable _disposable;
        private readonly PlayerModel _playerModel;
        private readonly GameSession _gameSession;
        private readonly ISubscriber<EntityDied> _entityDiedSubscriber;
        private readonly ISubscriber<DamageApplied> _damageAppliedSubscriber;

        public BattleStatisticsService(PlayerModel playerModel, GameSession gameSession,
            ISubscriber<EntityDied> entityDiedSubscriber,
            ISubscriber<DamageApplied> damageAppliedSubscriber)
        {
            _playerModel = playerModel;
            _gameSession = gameSession;
            _entityDiedSubscriber = entityDiedSubscriber;
            _damageAppliedSubscriber = damageAppliedSubscriber;
        }

        public void Initialize()
        {
            _gameSession.BattleStats = new BattleStats();
            _disposable = DisposableBag.Create(
                _entityDiedSubscriber.Subscribe(OnEntityDied),
                _damageAppliedSubscriber.Subscribe(OnDamageApplied));
        }

        private void OnEntityDied(EntityDied entityDied)
        {
            if (entityDied.Target != _playerModel.Unit)
                _gameSession.BattleStats.EnemyKilled++;
        }

        private void OnDamageApplied(DamageApplied damageApplied)
        {
            if (damageApplied.Target == _playerModel.Unit)
                _gameSession.BattleStats.DamageReceived += damageApplied.Amount;
            if (damageApplied.Source == _playerModel.Unit)
                _gameSession.BattleStats.DamageDealt += damageApplied.Amount;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}