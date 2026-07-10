using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerInput : MonoBehaviour
    {
        [SerializeField] private int _keyboardInputSpeed = 1;
        private readonly Vector3 _zeroVector3 = Vector3.zero;
        private readonly Vector3 _rightVector3 = Vector3.right;
        private readonly Vector3 _leftVector3 = Vector3.left;
        private Vector3 _moveDirection = Vector3.zero;
        private float _touchDeltaThreshold;
        private float _swipeDeltaThreshold;

        public void Initialize(float thrX, float thrSwipe)
        {
            _touchDeltaThreshold = thrX;
            _swipeDeltaThreshold = thrSwipe;
        }

        private void Update()
        {
            var controller = RunnerContext.GameController;
            var player = RunnerContext.Player;
            if (controller == null || player == null || !controller.canUserMove)
                return;

            if (RunnerContext.TutorialGate.IsBlockingInput)
                return;

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _moveDirection = _rightVector3 * _keyboardInputSpeed;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                _moveDirection = _leftVector3 * _keyboardInputSpeed;
            }
            else
            {
                _moveDirection = _zeroVector3;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (RunnerContext.TutorialGate.CanUseVerticalFormation)
                {
                    RunnerContext.TutorialGate.NotifyFormationUsed(true);
                    player.SetFormation(RunnerFormation.Vertical);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (RunnerContext.TutorialGate.CanUseHorizontalFormation)
                {
                    RunnerContext.TutorialGate.NotifyFormationUsed(false);
                    player.SetFormation(RunnerFormation.Horizontal);
                }
            }
#else
            if (Input.touchCount > 0)
            {
                Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;

                if (Mathf.Abs(deltaPosition.y) > _swipeDeltaThreshold)
                {
                    if (deltaPosition.y > 0)
                    {
                        if (RunnerContext.TutorialGate.CanUseVerticalFormation)
                        {
                            RunnerContext.TutorialGate.NotifyFormationUsed(true);
                            player.SetFormation(RunnerFormation.Vertical);
                        }
                    }
                    else
                    {
                        if (RunnerContext.TutorialGate.CanUseHorizontalFormation)
                        {
                            RunnerContext.TutorialGate.NotifyFormationUsed(false);
                            player.SetFormation(RunnerFormation.Horizontal);
                        }
                    }
                }
                else
                {
                    if (Mathf.Abs(deltaPosition.x) > _touchDeltaThreshold)
                        _moveDirection.x = deltaPosition.x;
                    else
                        _moveDirection = _zeroVector3;
                }
            }
            else
            {
                _moveDirection = _zeroVector3;
            }
#endif
        }

        public Vector3 GetMoveDirection()
        {
            return _moveDirection;
        }
    }
}
