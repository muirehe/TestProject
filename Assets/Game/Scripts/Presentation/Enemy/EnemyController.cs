using Gameplay.Combat;
using UnityEngine;

namespace Presentation.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        private Unit _unit;
        
        public void Bind(Unit unit) => _unit = unit;
    }
}