using Gameplay.Combat;
using UnityEngine;

namespace Presentation
{
    public abstract class UnitView : MonoBehaviour
    {
        public Unit Unit { get; private set; }

        public void Init(Unit unit)
        {
            Unit = unit;
            OnInit(unit);
        }

        protected abstract void OnInit(Unit unit);
    }
}