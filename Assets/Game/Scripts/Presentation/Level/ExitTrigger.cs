using System;
using Gameplay.Battle;
using MessagePipe;
using Presentation.Player;
using TMPro;
using UnityEngine;
using VContainer;

namespace Presentation.Level
{
    public class ExitTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private TextMeshPro exitText;
        [SerializeField] private Transform exitTextTr;

        public event Action PlayerEntered;
        
        private bool _isActive;
        private ISubscriber<BattleEnded> _subscriber;
        private IDisposable _disposable;

        [Inject]
        private void Inject(ISubscriber<BattleEnded> subscriber)
        {
            _subscriber = subscriber;
            _disposable = _subscriber.Subscribe(OnBattleEnded);
        }

        private void Awake()
        {
            SetState(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActive && other.GetComponentInParent<PlayerView>() != null)
            {
                SetState(false);
                PlayerEntered?.Invoke();
            }
        }

        private void Update()
        {
            if (_isActive)
                exitTextTr.RotateAround(exitTextTr.position, Vector3.up, 30 * Time.deltaTime);
        }

        private void OnBattleEnded(BattleEnded obj)
        {
            if (!obj.IsVictory) return;
            SetState(true);
        }

        private void SetState(bool isActive)
        {
            _isActive = isActive;
            boxCollider.enabled = isActive;
            exitText.gameObject.SetActive(isActive);
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}