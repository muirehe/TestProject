using Configs;
using Infrastructure.ConfigSystem;
using UnityEngine;
using VContainer;

namespace Presentation.Player
{
    public class WalkSwayingMaker : MonoBehaviour
    {
        [field: SerializeField] public CharacterController characterController;
        [field: SerializeField] public Transform cameraRoot;

        private Vector3 _startPosition;
        private float _phase;

        private WalkSwayingSettings _settings;

        [Inject]
        private void Inject(ConfigProvider configProvider)
        {
            _settings = configProvider.GetSingle<WalkSwayingSettings>();
        }

        private void Awake() => _startPosition = cameraRoot.localPosition;

        private void LateUpdate()
        {
            var velocity = characterController.velocity;
            var speed = new Vector2(velocity.x, velocity.z).magnitude;
            var target = _startPosition;

            if (characterController.isGrounded && speed > 0.1f)
            {
                _phase += speed * _settings.StepFrequencyInRadPerMeter * Time.deltaTime;
                target += new Vector3(Mathf.Cos(_phase * 0.5f) * _settings.Amplitude * 0.5f,
                    Mathf.Sin(_phase) * _settings.Amplitude, 0f);
            }

            cameraRoot.localPosition = Vector3.Lerp(cameraRoot.localPosition, target,
                1f - Mathf.Exp(-_settings.Smoothing * Time.deltaTime));
        }
    }
}
