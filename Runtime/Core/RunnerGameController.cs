using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerGameController : MonoBehaviour
    {
        public static RunnerGameController instance;

        [SerializeField] private RunnerEngineSettings _settings;
        [SerializeField] private RunnerEvents _events;
        [SerializeField] private ColorPicker _playerStartColor;
        [SerializeField] private int _startSize = 3;
        [SerializeField] private float _changingPositionTime = 2f;

        public bool playerMoving = true;
        public bool canUserMove = true;
        public bool shouldFormationSwitch;
        public bool isGameStarted;

        private int _coinsCollectedThisTurn;
        private float _currentMultiplier = 1f;
        private int _foodCollectedThisTurn;

        protected virtual void Awake()
        {
            instance = this;
            RunnerContext.GameController = this;

            if (_settings != null)
                RunnerContext.Settings = _settings;
            if (_events != null)
                RunnerContext.Events = _events;

            if (_settings != null)
                Application.targetFrameRate = _settings.targetFrameRate;
        }

        protected virtual void Start()
        {
            if (RunnerContext.Player == null)
            {
                Debug.LogError("[RunnerGameController] RunnerContext.Player is not set.");
                return;
            }

            RunnerContext.Player.SetInitialColor(_playerStartColor);

            if (shouldFormationSwitch)
                StartGoingBetweenPositions();
        }

        private void Update()
        {
            if (!isGameStarted && Input.GetMouseButtonDown(0))
            {
                isGameStarted = true;
                playerMoving = true;
                canUserMove = true;
                RunnerContext.Events?.onRunStarted?.Invoke();
            }
        }

        protected void StartGoingBetweenPositions()
        {
            StartCoroutine(nameof(SwitchPositionsRoutine));
        }

        private IEnumerator SwitchPositionsRoutine()
        {
            float interval = _settings != null ? _settings.formationSwitchInterval : _changingPositionTime;
            while (true)
            {
                yield return new WaitForSeconds(interval / 2f);
                RunnerContext.Player?.SetFormation(RunnerFormation.Straight);
                yield return new WaitForSeconds(interval / 2f);
                RunnerContext.Player?.SetFormation(RunnerFormation.Horizontal);
            }
        }

        public void StopGoingBetweenPositions()
        {
            StopCoroutine(nameof(SwitchPositionsRoutine));
        }

        public void IncreaseCoinCollected()
        {
            _coinsCollectedThisTurn += 1;
        }

        public void GameOver()
        {
            canUserMove = false;
            playerMoving = false;
            RunnerContext.Events?.onRunFailed?.Invoke();
        }

        public void HitWrongThing()
        {
            RunnerContext.Events?.onWrongCollectible?.Invoke();
        }

        public void LevelDone()
        {
            StartCoroutine(LevelDoneRoutine());
        }

        private IEnumerator LevelDoneRoutine()
        {
            int currentFinalScore = Mathf.RoundToInt(_foodCollectedThisTurn * _currentMultiplier);
            int earned = currentFinalScore + _coinsCollectedThisTurn;

            var result = new RunResult
            {
                foodScore = currentFinalScore,
                coinsCollected = _coinsCollectedThisTurn,
                multiplier = _currentMultiplier,
                totalEarned = earned
            };

            yield return new WaitForSeconds(1.5f);

            if (RunnerContext.FinishLine != null && RunnerContext.Player != null)
            {
                RunnerContext.FinishLine.SetMultiplierParticlePosition(RunnerContext.Player.LastSectorPosition);
            }

            yield return new WaitForSeconds(1f);
            RunnerContext.Events?.onRunCompleted?.Invoke(result);
        }

        public void LoadNextScene()
        {
            RunnerContext.Events?.onRequestNextLevel?.Invoke();
        }

        public void SetPlayerColor(ColorPicker color)
        {
            _playerStartColor = color;
            RunnerContext.Player?.SetInitialColor(color);
        }

        public void SetLatestMultiplier(float multiplier)
        {
            _currentMultiplier = multiplier;
        }

        public void IncreaseFoodCollectedThisTurn()
        {
            _foodCollectedThisTurn += 1;
            RunnerContext.Events?.onFoodCollected?.Invoke(_foodCollectedThisTurn);
        }

        public void DecreaseFoodCollectedThisTurn()
        {
            _foodCollectedThisTurn -= 1;
            _foodCollectedThisTurn = Mathf.Max(0, _foodCollectedThisTurn);
            RunnerContext.Events?.onFoodDecreased?.Invoke(_foodCollectedThisTurn);
        }
    }
}
