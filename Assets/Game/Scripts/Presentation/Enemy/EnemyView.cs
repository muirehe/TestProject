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
        [SerializeField] private EnemyAttackController enemyAttackController;
        [SerializeField] private HealthView healthView;
        [SerializeField] private AbilityController abilityController;

        protected override void OnInit(Unit unit)
        {
            enemyMoveController.Bind(unit);
            enemyAttackController.Bind(unit);
            healthView.Bind(unit);
            abilityController.Bind(unit);
        }
        
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}