using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Combat;
using Gameplay.Player;
using MessagePipe;
using Meta;
using Presentation.Enemy;
using UnityEngine;
using Utils;
using VContainer;
using VContainer.Unity;

namespace Presentation.Level.Battle
{
    public class BattleController : IInitializable, IDisposable
    {
        private readonly List<EnemyView> _enemies = new();
        private IDisposable _disposable;

        private readonly LevelView _levelView;
        private readonly PlayerModel _playerModel;
        private readonly GameSession _gameSession;
        private readonly IObjectResolver _objectResolver;
        private readonly ISubscriber<EntityDied> _entityDiedSubscriber;
        private readonly IPublisher<BattleEnded> _battleEndedPublisher;
        private readonly IPublisher<ExitEntered> _exitEnteredPublisher;

        public BattleController(GameSession gameSession, LevelView levelView, PlayerModel playerModel,
            IObjectResolver objectResolver, ISubscriber<EntityDied> subscriber,
            IPublisher<BattleEnded> publisher, IPublisher<ExitEntered> exitEnteredPublisher)
        {
            _gameSession = gameSession;
            _playerModel = playerModel;
            _levelView = levelView;
            _objectResolver = objectResolver;
            _entityDiedSubscriber = subscriber;
            _battleEndedPublisher = publisher;
            _exitEnteredPublisher = exitEnteredPublisher;
        }

        public void Initialize()
        {
            _disposable = DisposableBag.Create(_entityDiedSubscriber.Subscribe(OnEntityDied));
            _levelView.SpawnTrigger.PlayerEntered += SpawnEnemies;
            _levelView.ExitTrigger.PlayerEntered += OnExitEntered;
        }

        private void SpawnEnemies()
        {
            if (_levelView.SpawnPoints.Count == 0)
            {
                Debug.LogError("[Battle] spawn points count is 0");
                return;
            }

            if (_gameSession.LevelConfig.EnemyMight <= 0)
            {
                Debug.LogError("[Battle] enemy might is 0");
                return;
            }

            var zeroMightEnemies =
                _gameSession.LevelConfig.Enemies.Where(e => e.Might <= 0f).Select(x => x.Id).ToList();
            if (zeroMightEnemies.Count > 0)
            {
                Debug.LogError(
                    $"[Battle] enemy {string.Join(" ", zeroMightEnemies)} with Might <= 0 in level {_gameSession.LevelConfig.name}");
                return;
            }

            var might = 0f;
            int index = 0;
            while (might < _gameSession.LevelConfig.EnemyMight)
            {
                var enemyConfig = _gameSession.LevelConfig.Enemies.GetRandomByWeight();
                var spawnPoint = _levelView.SpawnPoints[index++ % _levelView.SpawnPoints.Count];
                var enemyView = _objectResolver.Instantiate(enemyConfig.Prefab, spawnPoint.Transform.position,
                    spawnPoint.Transform.rotation);
                var enemy = new Unit(enemyConfig);
                enemyView.Init(enemy);
                _enemies.Add(enemyView);
                might += enemyConfig.Might;
            }
        }

        private void OnEntityDied(EntityDied entityDied)
        {
            if (entityDied.Target == _playerModel.Unit)
            {
                EndBattle(false);
                return;
            }

            var enemy = _enemies.FirstOrDefault(x => x.Unit == entityDied.Target);
            if (enemy == null) return;
            _enemies.Remove(enemy);
            enemy.Die();
            if (_enemies.Count == 0)
                EndBattle(true);
        }

        private void EndBattle(bool isVictory)
        {
            _battleEndedPublisher.Publish(new BattleEnded(isVictory));
        }
        
        private void OnExitEntered() =>  _exitEnteredPublisher.Publish(new ExitEntered());

        public void Dispose()
        {
            _disposable?.Dispose();
            _levelView.SpawnTrigger.PlayerEntered -= SpawnEnemies;
            _levelView.ExitTrigger.PlayerEntered -= OnExitEntered;
        }
    }
}