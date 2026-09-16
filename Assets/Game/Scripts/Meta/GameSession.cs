using Configs;

namespace Meta
{
    public class GameSession
    {
        public LevelConfig LevelConfig { get; set; }
        public bool IsVictory { get; set; }
        public BattleStats BattleStats { get; set; } = new();
    }

    public class BattleStats
    {
        public int EnemyKilled { get; set; }
        public float DamageDealt { get; set; }
        public float DamageReceived { get; set; }
        public float StartTime { get; set; }
        public float Duration { get; set; }
        public int RewardCoins { get; set; }
        public int RewardExp { get; set; }
    }
}