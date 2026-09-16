using System.Collections.Generic;
using Infrastructure.ConfigSystem;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : DefinitionConfig, IDefinitionById
    {
        [field: SerializeField] public List<EnemyConfig> Enemies { get; private set; }
        [field: SerializeField] public float EnemyMight { get; private set; }
        public string Id => name;
    }
}