using Infrastructure.GameStateMachine;
using Infrastructure.GameStateMachine.States;
using Meta;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Presentation.Windows
{
    public class ResultsWindow : WindowBase
    {
        [SerializeField] private Button toMenuButton;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI enemyKilledCountText;
        [SerializeField] private TextMeshProUGUI damageDealtCountText;
        [SerializeField] private TextMeshProUGUI damageReceivedCountText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI coinsText, expText;

        [Inject] private IGameStateMachine _gameStateMachine;
        [Inject] private GameSession _gameSession;
        
        private void Start()
        {
            toMenuButton.onClick.AddListener(OnToMenuButtonClicked);
        }

        private void OnToMenuButtonClicked()
        {
            _gameStateMachine.Enter<MenuState>();
        }

        public override void Show(params object[] args)
        {
            titleText.text = _gameSession.IsVictory ? "Victory!" : "Defeat!";
            enemyKilledCountText.text = _gameSession.BattleStats.EnemyKilled.ToString();
            damageDealtCountText.text = _gameSession.BattleStats.DamageDealt.ToString("F0");
            damageReceivedCountText.text = _gameSession.BattleStats.DamageReceived.ToString("F0");
            timeText.text = _gameSession.BattleStats.Duration.ToString("F1");
            coinsText.text = _gameSession.BattleStats.RewardCoins.ToString();
            expText.text = _gameSession.BattleStats.RewardExp.ToString();
        }

        public override void Hide()
        {
        }
    }
}
