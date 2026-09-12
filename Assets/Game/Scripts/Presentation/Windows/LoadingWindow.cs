using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Windows
{
    public class LoadingWindow : WindowBase, IProgress<float>
    {
        [SerializeField] private Slider loadingSlider;
        [SerializeField] private TextMeshProUGUI progressText;

        public override void Show(params object[] args)
        {
            Report(0f);
        }

        public override void Hide()
        {
        }

        public void Report(float value)
        {
            loadingSlider.value = value;
            progressText.text = $"{value * 100f:F1}%";
        }
    }
}
