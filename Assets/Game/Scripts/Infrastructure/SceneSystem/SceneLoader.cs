using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.SceneSystem
{
    public class SceneLoader
    {
        public static string Boot = nameof(Boot);
        public static string Menu = nameof(Menu);
        public static string Gameplay = nameof(Gameplay);

        public async UniTask<bool> LoadAsync(string sceneName, IProgress<float> progress, CancellationToken token)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (operation == null)
            {
                Debug.LogError($"[SceneLoader] Scene '{sceneName}' not found in build settings.");
                return false;
            }

            while (!operation.isDone)
            {
                progress?.Report(Mathf.Clamp01(operation.progress / 0.9f));
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            progress?.Report(1f);
            return true;
        }
        
        public async UniTask<bool> LoadAsync(string sceneName, CancellationToken token)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (operation == null)
            {
                Debug.LogError($"[SceneLoader] Scene '{sceneName}' not found in build settings.");
                return false;
            }

            while (!operation.isDone)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            return true;
        }
    }
}