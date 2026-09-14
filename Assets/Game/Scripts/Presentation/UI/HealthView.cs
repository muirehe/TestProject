using System;
using Gameplay.Combat;
using MessagePipe;
using TMPro;
using UnityEngine;
using VContainer;

namespace Presentation.UI
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;

        private Unit _unit;
        private IDisposable _disposable;

        [Inject] private ISubscriber<DamageApplied> _damageAppliedSubscriber;

        public void Bind(Unit unit)
        {
            _unit = unit;
            _disposable?.Dispose();
            _disposable = DisposableBag.Create(_damageAppliedSubscriber.Subscribe(OnDamageApplied));
            Refresh();
        }

        private void OnDamageApplied(DamageApplied damageApplied)
        {
            if (_unit != damageApplied.Target) return;
            Refresh();
        }

        private void Refresh()
        {
            if (_unit == null) return;
            healthText.text = $"{Mathf.CeilToInt(_unit.Health)}/{Mathf.CeilToInt(_unit.MaxHealth)}";
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}