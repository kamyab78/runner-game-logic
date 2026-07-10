using DG.Tweening;
using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerFood : MonoBehaviour, ICollideWithPart, ICollideWithMouth
    {
        [SerializeField] private ColorPicker _color;
        [SerializeField] private float destroyTime = 0.2f;
        [SerializeField] private GameObject _fakeShadow;

        private Transform _transform;
        private Rigidbody _rigidbody;
        private MeshRenderer _meshRenderer;
        private Collider _collider;

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            if (RunnerContext.ColorProvider != null)
                _meshRenderer.material.SetColor("_BaseColor", RunnerContext.ColorProvider.GetColor(_color));
        }

        public ColorPicker GetColor()
        {
            return _color;
        }

        public void DestroyFood()
        {
            _collider.enabled = false;
            _transform.DOScale(Vector3.zero, destroyTime).OnComplete(() => Destroy(gameObject));
        }

        public void ThrowAway(Vector3 throwDirection)
        {
            _rigidbody.AddForce(throwDirection * 300);
            _collider.enabled = false;
            if (_fakeShadow != null)
                _fakeShadow.SetActive(false);
            Invoke(nameof(ThrowAwayScaleDown), .5f);
        }

        public void ThrowAwayScaleDown()
        {
            _transform.DOScale(Vector3.zero, 1).OnComplete(() => Destroy(gameObject));
        }

        public void SetColor(ColorPicker color)
        {
            _color = color;
        }

        public void CollideWithPart()
        {
            ThrowAway(new Vector3(transform.position.x / 5f, .35f, 1f));
        }

        public void CollideWithMouth()
        {
            var player = RunnerContext.Player;
            var controller = RunnerContext.GameController;
            var settings = RunnerContext.Settings;

            if (player == null || controller == null)
                return;

            bool colorMatch = !settings || !settings.enableColorMatching || GetColor() == player.GetColor();

            if (colorMatch)
            {
                RunnerContext.Vibration.VibratePop();
                controller.IncreaseFoodCollectedThisTurn();
                player.Grow();
                player.PlayCollectEffect(_transform.position, GetColor());
                DestroyFood();
            }
            else
            {
                RunnerContext.Vibration.VibratePeek();
                player.Shrink();
                controller.DecreaseFoodCollectedThisTurn();
                DestroyFood();
                controller.HitWrongThing();
            }
        }
    }
}
