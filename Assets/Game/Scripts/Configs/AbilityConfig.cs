using Infrastructure.ConfigSystem;
using UnityEngine;

namespace Configs
{
    public abstract class AbilityConfig : DefinitionConfig, IDefinitionById
    {
        [field: SerializeField] public float Cooldown { get; private set; }    
        public string Id => name;
    }
}