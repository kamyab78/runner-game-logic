using UnityEngine;

namespace HyperCasualRunner
{
    [CreateAssetMenu(fileName = "RunnerEngineSettings", menuName = "Hyper Casual Runner/Engine Settings")]
    public class RunnerEngineSettings : ScriptableObject
    {
        [Header("Level Generation")]
        public string levelResourcePath = "Levels/";
        public float lineX = 2.5f;
        public float startPositionZ = 10f;
        public float spaceBetweenEachLine = 2f;
        public float spaceBeforeAfterObstacles = 3f;
        public float horizontalObstaclePositionX = 2.5f;

        [Header("Prefabs")]
        public GameObject foodPrefab;
        public GameObject verticalObstaclePrefab;
        public GameObject horizontalObstaclePrefab;
        public GameObject finishLinePrefab;
        public GameObject coinPrefab;
        public GameObject coinInAirPrefab;
        public GameObject colorChangerPrefab;

        [Header("Run")]
        public int defaultStartSize = 3;
        public float formationSwitchInterval = 2f;
        public bool enableFormationSwitch;
        public bool enableColorMatching = true;
        public int targetFrameRate = 30;
    }
}
