using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerFinishSector : MonoBehaviour, ICollideWithMouth
    {
        [SerializeField] private List<ParticleSystem> _particleSystems;

        private MeshRenderer _meshRenderer;
        private TextMeshPro _text;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _text = GetComponentInChildren<TextMeshPro>();
        }

        public void Initialize(Color color, float text)
        {
            _meshRenderer.material.SetColor("_BaseColor", color);
            _text.text = text.ToString(".0#") + "x";
        }

        public void PlayParticles()
        {
            foreach (var particle in _particleSystems)
                particle.Play();
        }

        public float GetMultiplier()
        {
            string str = _text.text;
            return float.Parse(str.Remove(str.Length - 1, 1));
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void CollideWithMouth()
        {
            RunnerContext.Vibration.VibratePop();
            PlayParticles();
            RunnerContext.FinishLine?.DisableFinishLineParticle();
            RunnerContext.GameController?.SetLatestMultiplier(GetMultiplier());

            if (RunnerContext.Player != null)
                RunnerContext.Player.LastSectorPosition = GetPosition();

            RunnerContext.Events?.onMultiplierSelected?.Invoke();
        }
    }
}
