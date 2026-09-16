using System;
using Infrastructure.ConfigSystem;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EconomyStatisticConfig", menuName = "Configs/EconomyStatisticConfig")]
    public class EconomyStatisticConfig : DefinitionConfig, ISingleDefinition
    {
        [field: SerializeField] public RewardFormula Coins { get; private set; }
        [field: SerializeField] public RewardFormula Exp { get; private set; }
        [field: SerializeField] public float ParTime { get; private set; } = 90f;

        [field: Range(0f, 1f)]
        [field: SerializeField]
        public float LoseMultiplier { get; private set; } = 0.3f;
    }

    [Serializable]
    public class RewardFormula
    {
        [field: SerializeField] public float Base { get; private set; } = 20f;
        [field: SerializeField] public float PerKill { get; private set; } = 10f;
        [field: SerializeField] public float PerDealtDamage { get; private set; } = 0.01f;
        [field: SerializeField] public float SpeedBonus { get; private set; } = 30f;
        [field: SerializeField] public float NoDamageBonus { get; private set; } = 20f;
    }
}