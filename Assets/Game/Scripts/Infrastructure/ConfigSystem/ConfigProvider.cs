using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infrastructure.ConfigSystem
{
    public class ConfigProvider
    {
        private readonly Dictionary<Type, ISingleDefinition> _singles = new();
        private readonly Dictionary<Type, Dictionary<string, IDefinitionById>> _byId = new();

        public ConfigProvider()
        {
            LoadAllConfigs();
        }

        private void LoadAllConfigs()
        {
            var allConfigs = Resources.LoadAll<DefinitionConfig>("");

            foreach (var config in allConfigs)
            {
                switch (config)
                {
                    case ISingleDefinition single:
                    {
                        var type = config.GetType();
                        if (_singles.ContainsKey(type))
                            Debug.LogWarning($"Duplicate single config of type {type}. Overwriting.");
                        _singles[type] = single;
                        break;
                    }
                    case IDefinitionById byId:
                    {
                        var type = config.GetType();
                        while (type != null && type != typeof(DefinitionConfig))
                        {
                            if (!_byId.TryGetValue(type, out var dict))
                            {
                                dict = new Dictionary<string, IDefinitionById>();
                                _byId[type] = dict;
                            }

                            if (dict.ContainsKey(byId.Id))
                                Debug.LogWarning($"Duplicate config with Id '{byId.Id}' in type {type}. Overwriting.");
                            dict[byId.Id] = byId;
                            type = type.BaseType;
                        }

                        break;
                    }
                    default:
                        Debug.LogWarning($"Config {config.name} doesn't implement any known interface. Ignored.");
                        break;
                }
            }
        }

        public T GetSingle<T>() where T : class, ISingleDefinition
        {
            var type = typeof(T);
            if (_singles.TryGetValue(type, out var config))
                return config as T;
            throw new Exception($"Single config of type {type} not found in Resources.");
        }

        public T GetById<T>(string id) where T : class, IDefinitionById
        {
            var type = typeof(T);
            if (_byId.TryGetValue(type, out var dict) && dict.TryGetValue(id, out var config))
                return config as T;
            throw new Exception($"Config of type {type} with Id '{id}' not found.");
        }

        public IEnumerable<T> GetAll<T>() where T : class, IDefinitionById
        {
            var type = typeof(T);
            if (_byId.TryGetValue(type, out var dict))
                return dict.Values.Cast<T>();
            return Enumerable.Empty<T>();
        }

        public bool HasConfig<T>(string id) where T : class, IDefinitionById
        {
            var type = typeof(T);
            return _byId.TryGetValue(type, out var dict) && dict.ContainsKey(id);
        }
    }
}