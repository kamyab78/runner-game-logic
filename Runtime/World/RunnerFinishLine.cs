using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerFinishLine : MonoBehaviour
    {
        public static RunnerFinishLine instance;

        [SerializeField] private GameObject _finishLineParticle;
        [SerializeField] private List<RunnerFinishSector> _finishSectors;
        [SerializeField] private List<Color> _sectorColors;
        [SerializeField] private GameObject _multiplierSelectedParticle;

        protected virtual void Awake()
        {
            instance = this;
            RunnerContext.FinishLine = this;
        }

        private void Start()
        {
            InitializeSectors();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (IsPlayerHead(other))
                RunnerContext.Player?.IncreaseSpeedGradually();
        }

        protected virtual bool IsPlayerHead(Collider other)
        {
            return other.GetComponent<RunnerHeadTrigger>() != null;
        }

        private void InitializeSectors()
        {
            float multiplier = 1.1f;
            for (int i = 0; i < _finishSectors.Count; i++)
            {
                _finishSectors[i].Initialize(_sectorColors[i], multiplier);
                multiplier += .1f;
            }
        }

        public void DisableFinishLineParticle()
        {
            if (_finishLineParticle != null)
                _finishLineParticle.SetActive(false);
        }

        public void SetMultiplierParticlePosition(Vector3 pos)
        {
            if (_multiplierSelectedParticle == null)
                return;

            Vector3 origPos = _multiplierSelectedParticle.transform.position;
            origPos.z = pos.z;
            _multiplierSelectedParticle.transform.position = origPos;
            _multiplierSelectedParticle.GetComponent<ParticleSystem>()?.Play();
        }
    }
}
