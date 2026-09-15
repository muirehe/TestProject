using Gameplay.Combat;
using Presentation.UI;
using UnityEngine;

namespace Presentation.Enemy
{
    public class EnemyView : UnitView
    {
        [SerializeField] private EnemyMoveController enemyMoveController;
        [SerializeField] private EnemyAttackController enemyAttackController;
        [SerializeField] private HealthView healthView;

        protected override void OnInit(Unit unit)
        {
            enemyMoveController.Bind(unit);
            enemyAttackController.Bind(unit);
            healthView.Bind(unit);
        }
        
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}