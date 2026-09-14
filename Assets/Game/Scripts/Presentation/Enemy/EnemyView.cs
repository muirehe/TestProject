using Gameplay.Combat;
using Presentation.UI;
using UnityEngine;

namespace Presentation.Enemy
{
    public class EnemyView : UnitView
    {
        [SerializeField] private EnemyController enemyController;
        [SerializeField] private HealthView healthView;

        protected override void OnInit(Unit unit)
        {
            enemyController.Bind(unit);
            healthView.Bind(unit);
        }
        
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}