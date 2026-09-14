using System.Collections.Generic;
using UnityEngine;

namespace Presentation.Level
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private List<SpawnPoint> spawnPoints;
        [SerializeField] private SpawnTrigger spawnTrigger;
        
        public List<SpawnPoint> SpawnPoints => spawnPoints;
        public SpawnTrigger SpawnTrigger => spawnTrigger;
    }
}