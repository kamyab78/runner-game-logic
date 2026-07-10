using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerCoin : MonoBehaviour, ICollideWithPart, ICollideWithMouth
    {
        private Transform _transform;
        private Collider _collider;
        private Renderer _renderer;

        private float _rotateSpeed = 150f;
        private bool _collected;

        private void Awake()
        {
            _transform = transform;
            _collider = GetComponent<Collider>();
            _renderer = GetComponentInChildren<Renderer>();
        }

        private void Update()
        {
            if (_renderer != null && !_renderer.isVisible)
                return;

            _transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
        }

        public void Collect()
        {
            if (_collected)
                return;
            _collected = true;
            _collider.enabled = false;
            RunnerContext.GameController?.IncreaseCoinCollected();
            RunnerContext.Events?.onCoinCollectedAtPosition?.Invoke(_transform.position);
            Destroy(gameObject);
        }

        public void CollideWithPart()
        {
            Collect();
        }

        public void CollideWithMouth()
        {
            Collect();
        }
    }
}
