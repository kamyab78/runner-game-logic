using UnityEngine;

namespace HyperCasualRunner
{
    /// <summary>
    /// Attach to the player head trigger collider. Dispatches ICollideWithMouth on other objects.
    /// </summary>
    public class RunnerHeadTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var collideWithMouth = other.GetComponent<ICollideWithMouth>();
            collideWithMouth?.CollideWithMouth();
        }
    }
}
