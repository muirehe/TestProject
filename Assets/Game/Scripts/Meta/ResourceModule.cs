using Configs;
using Gameplay.Combat;
using Infrastructure.ConfigSystem;
using Infrastructure.SaveSystem;
using MessagePack;
using UnityEngine;

namespace Meta
{
    [MessagePackObject]
    public class ResourceData
    {
        [Key(0)] public int Coins { get; set; }
        [Key(1)] public int Experience { get; set; }
    }

    public class ResourceModule : ModuleBase<ResourceData>
    {
        private readonly PlayerConfig _playerConfig;
        private readonly EconomyStatisticConfig _economyStatisticConfig;

        public ResourceModule(ConfigProvider configProvider)
        {
            _economyStatisticConfig = configProvider.GetSingle<EconomyStatisticConfig>();
            _playerConfig = configProvider.GetSingle<PlayerConfig>();
        }

        public (int coins, int exp) AddResourcesFromBattle(BattleStats battleStats, bool isVictory)
        {
            var coins = Calculate(_economyStatisticConfig.Coins,
                battleStats, isVictory, _economyStatisticConfig.ParTime,
                ModifierCalculator.ProcessModifiers(_playerConfig, StatType.MaxHealth),
                _economyStatisticConfig.LoseMultiplier);
            var exp = Calculate(_economyStatisticConfig.Exp,
                battleStats, isVictory, _economyStatisticConfig.ParTime,
                ModifierCalculator.ProcessModifiers(_playerConfig, StatType.MaxHealth),
                _economyStatisticConfig.LoseMultiplier);
            Data.Coins += coins;
            Data.Experience += exp;
            return (coins, exp);
        }

        private static int Calculate(RewardFormula rewardFormula, BattleStats battleStats, bool isVictory,
            float parTime,
            float maxHealth,
            float loseMultiplier)
        {
            var value = rewardFormula.Base + rewardFormula.PerKill * battleStats.EnemyKilled +
                        rewardFormula.PerDealtDamage * battleStats.DamageDealt;
            if (isVictory)
            {
                value += rewardFormula.SpeedBonus * Mathf.Clamp01(1 - battleStats.Duration / parTime);
                value += rewardFormula.NoDamageBonus * Mathf.Clamp01(1 - battleStats.DamageReceived / maxHealth);
            }
            else
            {
                value *= loseMultiplier;
            }

            return Mathf.Max(0, Mathf.RoundToInt(value));
        }
    }
}