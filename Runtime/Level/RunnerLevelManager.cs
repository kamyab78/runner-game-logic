using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerLevelManager : MonoBehaviour
    {
        public static RunnerLevelManager instance;

        private readonly List<TextAsset> _levels = new List<TextAsset>();
        private string _levelResourcePath = "Levels/";

        protected virtual void Awake()
        {
            instance = this;
            if (RunnerContext.Settings != null)
                _levelResourcePath = RunnerContext.Settings.levelResourcePath;
            LoadLevels();
        }

        public string GetCurrentLevel()
        {
            int index = RunnerContext.LevelProgress != null
                ? RunnerContext.LevelProgress.GetCurrentLevelIndex()
                : 0;
            return GetLevel(index);
        }

        private void LoadLevels()
        {
            _levels.Clear();
            _levels.AddRange(Resources.LoadAll<TextAsset>(_levelResourcePath)
                .OrderBy(t => int.TryParse(t.name, out var n) ? n : int.MaxValue));
        }

        private string GetLevel(int level)
        {
            if (_levels.Count == 0)
            {
                Debug.LogError($"[RunnerLevelManager] No level files found in Resources/{_levelResourcePath}.");
                return string.Empty;
            }

            return _levels[Mathf.Clamp(level, 0, _levels.Count - 1)].text;
        }

        public int GetLastLevelNumber()
        {
            return _levels.Count - 1;
        }
    }
}
