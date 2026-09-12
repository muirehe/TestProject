using UnityEngine;

namespace Infrastructure.ConfigSystem
{
    public interface ISingleDefinition
    {
    }

    public interface IDefinitionById
    {
        string Id { get; }
    }

    public abstract class DefinitionConfig : ScriptableObject
    {
    }
}