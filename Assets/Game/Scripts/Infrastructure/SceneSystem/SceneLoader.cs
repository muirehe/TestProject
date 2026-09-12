using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.SceneSystem
{
    public class SceneLoader
    {
        public const string Boot = nameof(Boot);
        public const string Menu = nameof(Menu);
        public const string Gameplay = nameof(Gameplay);

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
    }
}