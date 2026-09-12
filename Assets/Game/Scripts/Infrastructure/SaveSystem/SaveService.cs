using System;
using System.Collections.Generic;
using System.IO;
using MessagePack;
using UnityEngine;

namespace Infrastructure.SaveSystem
{
    public class SaveService
    {
        private static string SavePath => Application.persistentDataPath;

        private readonly IEnumerable<IModule> _modules;

        public SaveService(IEnumerable<IModule> modules)
        {
            _modules = modules;
        }

        public event Action BeforeSave;
        public event Action AfterLoad;

        public void LoadAll() => LoadAll(SavePath);

        public void LoadAll(string dir)
        {
            var loaded = new List<(IModule module, object data)>();
            foreach (var module in _modules)
                loaded.Add((module, module.ReadData(dir)));
            foreach (var (module, data) in loaded)
                module.ApplyData(data);
            AfterLoad?.Invoke();
        }

        public void SaveAll(string dir)
        {
            BeforeSave?.Invoke();
            foreach (var module in _modules)
                module.Save(dir);
            Debug.Log($"Save: all modules saved in {dir}");
        }

        public void ResetAll()
        {
            foreach (var module in _modules)
                module.Reset();
            AfterLoad?.Invoke();
            Debug.Log("Reset: all modules reset");
        }

        internal static T LoadData<T>(string dir, string fileName) where T : class, new()
        {
            string path = Path.Combine(dir, fileName);
            if (!File.Exists(path))
                return new T();

            byte[] bytes = File.ReadAllBytes(path);
            try
            {
                return MessagePackSerializer.Deserialize<T>(bytes);
            }
            catch (Exception e)
            {
                Debug.LogError($"Save: failed to load {fileName}, starting fresh. {e}");
                return new T();
            }
        }

        internal static void SaveData<T>(string dir, string fileName, T state) where T : class
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            Directory.CreateDirectory(dir);
            byte[] bytes = MessagePackSerializer.Serialize(state);
            File.WriteAllBytes(Path.Combine(dir, fileName), bytes);
        }
    }
}