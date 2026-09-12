using System.Linq;
using Configs;
using Infrastructure.ConfigSystem;
using Infrastructure.Input;
using UnityEngine;
using VContainer;

namespace Presentation.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private Transform tr;

        private float _pitch;
        private float _verticalVelocity;

        private GameInput _gameInput;
        private InputSettings _inputSettings;

        [Inject]
        private void Inject(GameInput gameInput, ConfigProvider configProvider)
        {
            _gameInput = gameInput;
            _inputSettings = configProvider.GetSingle<InputSettings>();
        }

        private void Update()
        {
            Look();
            Move();
        }

        private void Look()
        {
            var delta = _gameInput.Player.Look.ReadValue<Vector2>() * _inputSettings.CursorSensitivity;

            tr.Rotate(0f, delta.x, 0f);
            _pitch = Mathf.Clamp(_pitch - delta.y, -_inputSettings.MaxPitch, _inputSettings.MaxPitch);
            cameraRoot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }

        private void Move()
        {
            var input = _gameInput.Player.Move.ReadValue<Vector2>();
            var horizontal = (tr.right * input.x + tr.forward * input.y) * _inputSettings.MoveSpeed;

            if (characterController.isGrounded && _verticalVelocity < 0f)
                _verticalVelocity = _inputSettings.GroundedVelocity;

            if (characterController.isGrounded && _gameInput.Player.Jump.WasPressedThisFrame())
                _verticalVelocity = Mathf.Sqrt(2f * _inputSettings.Gravity * _inputSettings.JumpMaxHeight);

            _verticalVelocity -= _inputSettings.Gravity * Time.deltaTime;
            characterController.Move((horizontal + Vector3.up * _verticalVelocity) * Time.deltaTime);
        }
    }
}