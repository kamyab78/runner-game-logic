using DG.Tweening;
using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerCameraController : MonoBehaviour
    {
        public static RunnerCameraController instance;

        [SerializeField] private Vector3 _targetOffset;
        [SerializeField] private float _smoothFollowSpeed = 5f;
        [SerializeField] private float _shakeDuration = 0.3f;
        [SerializeField] private float _shakeStrength = 0.5f;

        private Transform _transform;
        private Camera _camera;

        protected virtual void Awake()
        {
            instance = this;
            RunnerContext.Camera = this;
            _transform = transform;
            _camera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            var controller = RunnerContext.GameController;
            var player = RunnerContext.Player;
            if (controller == null || player == null || !controller.playerMoving)
                return;

            _transform.position = Vector3.Lerp(
                _transform.position,
                player.Transform.position + _targetOffset,
                _smoothFollowSpeed * Time.deltaTime);
        }

        public void ShakeCamera()
        {
            if (_camera != null)
                _camera.DOShakePosition(_shakeDuration, _shakeStrength);
        }
    }
}
