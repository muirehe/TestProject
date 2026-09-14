using Infrastructure.ConfigSystem;
using Presentation.Enemy;
using UnityEngine;
using Utils;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
    public class EnemyConfig : UnitConfig, IDefinitionById, IRandomByWeight
    {
        [field: SerializeField] public float Might { get; private set; }
        [field: SerializeField] public int Weight { get; private set; }
        [field: SerializeField] public EnemyView Prefab { get; private set; }
        public string Id => name;
    }
}