using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Presentation
{
    public interface IWindow
    {
        void Show(params object[] args);
        void Hide();
    }

    public abstract class WindowBase : MonoBehaviour, IWindow
    {
        public abstract void Show(params object[] args);
        public abstract void Hide();
    }

    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private Transform windowRoot;
        
        [Inject] private IObjectResolver _globalResolver;
        private Dictionary<Type, WindowBase> _prefabCache;
        private Dictionary<Type, IWindow> _activeWindows;
        private bool _prefabsIndexed;
        
        private void Awake()
        {
            _activeWindows = new Dictionary<Type, IWindow>();
            _prefabCache = new Dictionary<Type, WindowBase>();
        }

        public TWindow Show<TWindow>(IObjectResolver resolver = null, params object[] args)
            where TWindow : MonoBehaviour, IWindow
        {
            var objectResolver = resolver ?? _globalResolver;
            var type = typeof(TWindow);

            if (_activeWindows.TryGetValue(type, out var existing))
            {
                existing.Show(args);
                return (TWindow)existing;
            }

            var prefab = GetPrefab(type);
            var instance = Instantiate(prefab, windowRoot);

            objectResolver.InjectGameObject(instance.gameObject);

            var window = instance.GetComponent<TWindow>();
            if (window == null)
                throw new InvalidOperationException($"Prefab for {type.Name} missing component {typeof(TWindow).Name}");

            _activeWindows[type] = window;
            window.Show(args);
            return window;
        }

        public bool IsShown<TWindow>() where TWindow : IWindow => _activeWindows.ContainsKey(typeof(TWindow));

        public void Hide<TWindow>() where TWindow : IWindow
        {
            var type = typeof(TWindow);
            if (_activeWindows.TryGetValue(type, out var window))
            {
                HideInternal(window, type);
            }
        }

        public void HideAll()
        {
            foreach (var window in new List<KeyValuePair<Type, IWindow>>(_activeWindows))
            {
                HideInternal(window.Value, window.Key);
            }
        }

        public void Hide(IWindow window)
        {
            var type = window.GetType();
            if (_activeWindows.TryGetValue(type, out var w) && w == window)
                HideInternal(window, type);
        }

        private void HideInternal(IWindow window, Type type)
        {
            window.Hide();
            _activeWindows.Remove(type);
            Destroy(((MonoBehaviour)window).gameObject);
        }

        private WindowBase GetPrefab(Type windowType)
        {
            if (_prefabCache.TryGetValue(windowType, out var prefab))
                return prefab;

            IndexAllWindowPrefabs();
            if (_prefabCache.TryGetValue(windowType, out prefab))
                return prefab;

            throw new Exception($"No prefab found in Resources with component {windowType.Name}");
        }

        private void IndexAllWindowPrefabs()
        {
            if (_prefabsIndexed) return;
            _prefabsIndexed = true;

            var windows = Resources.LoadAll<WindowBase>("");
            foreach (var window in windows)
            {
                var type = window.GetType();
                _prefabCache.TryAdd(type, window);
            }
        }
    }
}