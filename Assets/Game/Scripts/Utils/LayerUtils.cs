using UnityEngine;

namespace Utils
{
    public static class LayerUtils
    {
        public static readonly int PlayerMask = LayerMask.GetMask("Player");
        public static readonly int EnemyMask = LayerMask.GetMask("Enemy");
        public static readonly int ObstacleMask = LayerMask.GetMask("Default");
    }
}