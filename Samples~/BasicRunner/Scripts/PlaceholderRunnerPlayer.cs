using HyperCasualRunner;
using UnityEngine;

namespace HyperCasualRunner.Samples
{
    /// <summary>
    /// Minimal IRunnerPlayer for bootstrapping a new hyper-casual game.
    /// Replace growth, visuals, and speed logic with your own theme.
    /// </summary>
    [RequireComponent(typeof(RunnerMotor))]
    [RequireComponent(typeof(RunnerInput))]
  public class PlaceholderRunnerPlayer : MonoBehaviour, IRunnerPlayer
    {
        [SerializeField] private float _forwardSpeed = 8f;
        [SerializeField] private float _horizontalSpeed = 10f;
        [SerializeField] private float _horizontalLimit = 4f;
        [SerializeField] private float _touchThreshold = 1f;
        [SerializeField] private float _swipeThreshold = 30f;

        private RunnerMotor _motor;
        private RunnerInput _input;
        private Transform _transform;
        private ColorPicker _color = ColorPicker.Green;
        private Vector3 _lastSectorPosition;

        public Transform Transform => _transform;
        public float ForwardSpeed { get => _forwardSpeed; set => _forwardSpeed = value; }
        public bool CanMove { get; set; } = true;
        public RunnerFormation Formation { get; set; } = RunnerFormation.Straight;
        public Vector3 LastSectorPosition { get => _lastSectorPosition; set => _lastSectorPosition = value; }

        private void Awake()
        {
            _transform = transform;
            _motor = GetComponent<RunnerMotor>();
            _input = GetComponent<RunnerInput>();
            _motor.Initialize(_horizontalLimit);
            _input.Initialize(_touchThreshold, _swipeThreshold);
            RunnerContext.Player = this;
        }

        private void Update()
        {
            var controller = RunnerContext.GameController;
            if (controller == null || !controller.playerMoving)
                return;

            _motor.MoveForward(_transform, _forwardSpeed);
            if (controller.canUserMove)
                _motor.MoveHorizontal(_transform, _input.GetMoveDirection(), _horizontalSpeed);
        }

        public ColorPicker GetColor() => _color;

        public void SetInitialColor(ColorPicker color) => _color = color;

        public void ChangeColor(ColorPicker color) => _color = color;

        public float GetHorizontalLimit() => _horizontalLimit;

        public void Grow() { }

        public void Shrink()
        {
            RunnerContext.GameController?.GameOver();
        }

        public void RetargetParts() { }

        public void PlayCollectEffect(Vector3 position, ColorPicker color) { }

        public void OnObstacleHit(bool isHead)
        {
            if (isHead)
                RunnerContext.GameController?.GameOver();
        }

        public void MoveToCenter(float duration)
        {
            _motor.MoveHorizontalCenter(_transform, duration);
        }

        public void IncreaseSpeedGradually() { }

        public void DecreaseSpeedGradually()
        {
            RunnerContext.GameController?.LevelDone();
        }

        public void SetFormation(RunnerFormation formation)
        {
            Formation = formation;
        }
    }
}
