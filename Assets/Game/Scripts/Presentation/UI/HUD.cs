using Gameplay.Player;
using UnityEngine;
using VContainer;

namespace Presentation.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private HealthView healthView;
        
        [Inject] private PlayerModel _playerModel;

        private void Start()
        {
            healthView.Bind(_playerModel.Unit);
        }
    }
}