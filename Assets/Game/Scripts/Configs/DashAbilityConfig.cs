using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "DashAbilityConfig", menuName = "Configs/DashAbilityConfig")]
    public class DashAbilityConfig : AbilityConfig
    {
        [field: SerializeField] public float Distance { get; private set; } = 5f;
    }
}