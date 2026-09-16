using Infrastructure.ConfigSystem;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "WalkSwayingSettings", menuName = "Configs/WalkSwayingSettings")]
    public class WalkSwayingSettings : DefinitionConfig, ISingleDefinition
    {
        [field: SerializeField] public float StepFrequencyInRadPerMeter { get; private set; } = 2.5f;
        [field: SerializeField] public float Amplitude { get; private set; } = 0.05f;
        [field: SerializeField] public float Smoothing { get; private set; } = 10f;
    }
}