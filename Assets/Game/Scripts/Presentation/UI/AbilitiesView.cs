using System;
using System.Collections.Generic;
using Gameplay.Combat;
using Gameplay.Player;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Presentation.UI
{
    public class AbilitiesView : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private AbilityView abilityViewProto;

        private readonly List<AbilityView> _views = new();
        private IDisposable _disposable;
        
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private PlayerModel _playerModel;
        [Inject] private ISubscriber<AbilityUsed> _subscriber;

        private void Start()
        {
            abilityViewProto.gameObject.SetActive(false);
            
            foreach (var abilityConfig in _playerModel.Unit.Config.Abilities)
            {
                var view = _objectResolver.Instantiate(abilityViewProto, container);
                view.Setup(_playerModel.Unit, abilityConfig);
                _views.Add(view);
            }

            _disposable = _subscriber.Subscribe(OnAbilityUsed);
        }

        private void OnAbilityUsed(AbilityUsed abilityUsed)
        {
            if (abilityUsed.Unit !=_playerModel.Unit ) return;

            foreach (var view in _views)
            {
                if (view.AbilityConfig == abilityUsed.AbilityConfig)
                {
                    view.gameObject.SetActive(true);
                    break;
                }
            }
        }

        private void Update()
        {
            foreach (var view in _views)
            {
                if (_playerModel.Unit.GetAbilityCooldown(view.AbilityConfig) <= 0)
                    view.gameObject.SetActive(false);
            }
        }
        
        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}