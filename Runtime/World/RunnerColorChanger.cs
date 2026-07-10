using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerColorChanger : MonoBehaviour, ICollideWithMouth
    {
        [SerializeField] private ColorPicker _color;

        private MeshRenderer _meshRenderer;
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        public void SetColor(ColorPicker color)
        {
            _color = color;
        }

        private void Start()
        {
            if (RunnerContext.ColorProvider == null)
                return;

            Color realColor = RunnerContext.ColorProvider.GetColor(_color);
            _meshRenderer.material.SetColor("_BaseColor", realColor);
            if (_particleSystem != null)
            {
                ParticleSystem.MainModule settings = _particleSystem.main;
                settings.startColor = realColor;
            }
        }

        public ColorPicker GetColor()
        {
            return _color;
        }

        public void CollideWithMouth()
        {
            RunnerContext.Vibration.VibratePeek();
            RunnerContext.Player?.ChangeColor(GetColor());
        }
    }
}
