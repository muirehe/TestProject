using Gameplay.Combat;
using Presentation.Abilities;
using Presentation.Player;
using Presentation.UI;
using UnityEngine;

namespace Presentation.Enemy
{
    public class EnemyView : UnitView
    {
        [SerializeField] private EnemyMoveController enemyMoveController;
        [SerializeField] private HealthView healthView;
        [SerializeField] private EnemyAbilityController abilityController;

        protected override void OnInit(Unit unit)
        {
            enemyMoveController.Bind(unit);
            abilityController.Bind(unit);
            healthView.Bind(unit);
        }
        
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}