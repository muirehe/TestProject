using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using MessagePack;
using UnityEngine;

namespace Infrastructure.SaveSystem
{
    public class SaveService
    {
        private static string SavePath => Application.persistentDataPath;

#if UNITY_WEBGL && !UNITY_EDITOR
        // В WebGL файлы попадают в IndexedDB только после FS.syncfs (Plugins/WebGL/FileSync.jslib)
        [DllImport("__Internal")]
        private static extern void SyncFiles();
#endif

        private readonly IEnumerable<IModule> _modules;

        public SaveService(IEnumerable<IModule> modules)
        {
            _modules = modules;
        }

        public event Action BeforeSave;
        public event Action AfterLoad;

        public void LoadAll() => LoadAll(SavePath);
        public void SaveAll() => SaveAll(SavePath);

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
#if UNITY_WEBGL && !UNITY_EDITOR
            SyncFiles();
#endif
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