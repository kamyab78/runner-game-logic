using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerGainControl : MonoBehaviour, ICollideWithMouth
    {
        public void Tick()
        {
            var player = RunnerContext.Player;
            var controller = RunnerContext.GameController;
            if (player == null || controller == null)
                return;

            player.MoveToCenter(0.5f);
            controller.canUserMove = false;
            controller.StopGoingBetweenPositions();
            player.SetFormation(RunnerFormation.Straight);
        }

        public void CollideWithMouth()
        {
            Tick();
        }
    }
}
