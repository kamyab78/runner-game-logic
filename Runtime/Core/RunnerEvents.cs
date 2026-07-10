using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualRunner
{
    [CreateAssetMenu(fileName = "RunnerEvents", menuName = "Hyper Casual Runner/Runner Events")]
    public class RunnerEvents : ScriptableObject
    {
        public UnityEvent onRunStarted;
        public UnityEvent<int> onFoodCollected;
        public UnityEvent<int> onFoodDecreased;
        public UnityEvent<Vector3> onCoinCollectedAtPosition;
        public UnityEvent onRunFailed;
        public UnityEvent<RunResult> onRunCompleted;
        public UnityEvent onWrongCollectible;
        public UnityEvent onRequestNextLevel;
        public UnityEvent onMultiplierSelected;
    }
}
