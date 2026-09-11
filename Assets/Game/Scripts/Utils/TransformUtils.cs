using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utils
{
    public static class TransformUtils
    {
        public static T GetOrCreateView<T>(this Transform container, T template, List<T> views, ref int count,
            IObjectResolver objectResolver) where T : Component
        {
            if (count < views.Count)
            {
                var existingView = views[count];
                count++;
                return existingView;
            }

            var view = objectResolver != null
                ? objectResolver.Instantiate(template, container)
                : Object.Instantiate(template, container);
            views.Add(view);
            count++;
            return view;
        }
        
        public static T GetOrCreateView<T>(this Transform container, T template, List<T> views, ref int count) where T : Object
        {
            if (count < views.Count)
            {
                var existingView = views[count];
                count++;
                return existingView;
            }

            var view = Object.Instantiate(template, container);
            views.Add(view);
            count++;
            return view;
        }
    }
}