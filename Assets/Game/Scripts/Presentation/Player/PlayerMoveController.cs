using Configs;
using Gameplay.Combat;
using Infrastructure.ConfigSystem;
using Infrastructure.Input;
using UnityEngine;
using VContainer;

namespace Presentation.Player
{
    public class PlayerMoveController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform tr;
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private Camera playerCamera;

        private Unit _unit;
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
        
        public void Bind(Unit unit) => _unit = unit;

        private void Start()
        {
            playerCamera.fieldOfView = _inputSettings.FOV;
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
            var speed = _gameInput.Player.Sprint.IsPressed()
                ? _inputSettings.SprintSpeedMultiplier * _unit.Get(StatType.MoveSpeed)
                : _unit.Get(StatType.MoveSpeed);
            var horizontal = (tr.right * input.x + tr.forward * input.y) * speed;

            if (characterController.isGrounded && _verticalVelocity < 0f)
                _verticalVelocity = _inputSettings.GroundedVelocity;

            if (characterController.isGrounded && _gameInput.Player.Jump.WasPressedThisFrame())
                _verticalVelocity = Mathf.Sqrt(2f * _inputSettings.Gravity * _inputSettings.JumpMaxHeight);

            _verticalVelocity -= _inputSettings.Gravity * Time.deltaTime;
            characterController.Move((horizontal + Vector3.up * _verticalVelocity) * Time.deltaTime);
        }
    }
}