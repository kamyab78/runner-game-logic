using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerObstacle : MonoBehaviour, ICollideWithPart, ICollideWithMouth
    {
        public void CollideWithPart()
        {
            RunnerContext.GameController?.DecreaseFoodCollectedThisTurn();
            RunnerContext.Player?.RetargetParts();
            RunnerContext.GameController?.HitWrongThing();
        }

        public void CollideWithMouth()
        {
            RunnerContext.Vibration.VibratePeek();
            RunnerContext.Player?.OnObstacleHit(true);
        }
    }
}
