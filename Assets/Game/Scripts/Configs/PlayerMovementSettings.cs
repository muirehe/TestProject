using Infrastructure.ConfigSystem;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Configs/PlayerMovementSettings")]
    public class PlayerMovementSettings : DefinitionConfig, ISingleDefinition
    {
        [field: SerializeField] public float SprintSpeedMultiplier { get; private set; } = 2f;
        [field: SerializeField] public float CursorSensitivity { get; private set; } = 0.1f;
        [field: SerializeField] public float JumpMaxHeight { get; private set; } = 1f;
        [field: SerializeField] public float Gravity { get; private set; } = 9.8f;
        [field: SerializeField] public float MaxPitch  { get; private set; } = 85f;
        [field: SerializeField] public float GroundedVelocity  { get; private set; } = -2f;
        [field: SerializeField] public float FOV { get; private set; } = 60f;
    }
}
