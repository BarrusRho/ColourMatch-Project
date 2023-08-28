using System.Collections.Generic;
using UnityEngine;

namespace ColourMatch
{
    [CreateAssetMenu(fileName = "ObstaclePrefabs", menuName = "DodgyBoxes/ObstaclePrefabs")]
    public class ObstaclePrefabsSO : ScriptableObject
    {
        public List<Obstacle> obstaclePrefabs;
    }
}
