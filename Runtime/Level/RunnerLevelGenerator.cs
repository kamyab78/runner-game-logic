using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerLevelGenerator : MonoBehaviour
    {
        [SerializeField] private RunnerEngineSettings _settings;

        [Header("Fallback (used when settings asset is not assigned)")]
        [SerializeField] private float _lineX = 2.5f;
        [SerializeField] private float _startPositionZ = 10f;
        [SerializeField] private float _spaceBetweenEachLine = 2f;
        [SerializeField] private float _spaceBeforeAfterObstacles = 3f;
        [SerializeField] private float _horizontalObstaclePositionX = 2.5f;
        [SerializeField] private GameObject _food;
        [SerializeField] private GameObject _verticalObstacle;
        [SerializeField] private GameObject _horizontalObstacle;
        [SerializeField] private GameObject _finishLine;
        [SerializeField] private GameObject _coin;
        [SerializeField] private GameObject _coinInAir;
        [SerializeField] private GameObject _colorChanger;

        private Transform _transform;
        private float _currentZ;
        private float _currentX;

        private void Awake()
        {
            _transform = transform;
            if (_settings == null)
                _settings = RunnerContext.Settings;
            GenerateLevel();
        }

        private float LineX => _settings != null ? _settings.lineX : _lineX;
        private float StartPositionZ => _settings != null ? _settings.startPositionZ : _startPositionZ;
        private float SpaceBetweenEachLine => _settings != null ? _settings.spaceBetweenEachLine : _spaceBetweenEachLine;
        private float SpaceBeforeAfterObstacles => _settings != null ? _settings.spaceBeforeAfterObstacles : _spaceBeforeAfterObstacles;
        private float HorizontalObstaclePositionX => _settings != null ? _settings.horizontalObstaclePositionX : _horizontalObstaclePositionX;
        private GameObject FoodPrefab => _settings != null ? _settings.foodPrefab : _food;
        private GameObject VerticalObstaclePrefab => _settings != null ? _settings.verticalObstaclePrefab : _verticalObstacle;
        private GameObject HorizontalObstaclePrefab => _settings != null ? _settings.horizontalObstaclePrefab : _horizontalObstacle;
        private GameObject FinishLinePrefab => _settings != null ? _settings.finishLinePrefab : _finishLine;
        private GameObject CoinPrefab => _settings != null ? _settings.coinPrefab : _coin;
        private GameObject CoinInAirPrefab => _settings != null ? _settings.coinInAirPrefab : _coinInAir;
        private GameObject ColorChangerPrefab => _settings != null ? _settings.colorChangerPrefab : _colorChanger;

        private void GenerateLevel()
        {
            if (RunnerLevelManager.instance == null)
            {
                Debug.LogError("[RunnerLevelGenerator] RunnerLevelManager is missing.");
                return;
            }

            string level = RunnerLevelManager.instance.GetCurrentLevel();
            string[] lines = level.Split('\n');

            var levelLines = new List<string>();
            string settingsLine = null;

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed))
                    continue;

                int dash = trimmed.IndexOf('-');
                if (dash < 0)
                {
                    Debug.LogError($"[RunnerLevelGenerator] Skipping malformed level line (no '-'): '{line}'");
                    continue;
                }

                if (settingsLine == null)
                    settingsLine = trimmed.Substring(0, dash);

                levelLines.Add(trimmed.Substring(dash + 1));
            }

            if (string.IsNullOrEmpty(settingsLine))
            {
                Debug.LogError("[RunnerLevelGenerator] Level has no valid lines; aborting generation.");
                return;
            }

            RunnerContext.GameController?.SetPlayerColor(GetColorBasedOnChar(settingsLine[0]));

            _currentX = -LineX;
            _currentZ = StartPositionZ;

            for (int i = 0; i < levelLines.Count; i++)
            {
                _currentZ = StartPositionZ;

                for (int j = 0; j + 1 < levelLines[i].Length; j += 2)
                {
                    switch (levelLines[i][j])
                    {
                        case 'H':
                            CreateGap(int.Parse(levelLines[i][j + 1].ToString()));
                            break;
                        case 'C':
                            CreateBigGap(2);
                            if (i == 0)
                                CreateColorChanger(GetColorBasedOnChar(levelLines[i][j + 1]));
                            CreateBigGap(2);
                            break;
                        case 'A':
                            CreateBigGap(2);
                            CreateHorizontalObstacle(i, levelLines[i][j + 1] == '1');
                            CreateBigGap(2);
                            break;
                        case 'D':
                            CreateBigGap(2);
                            if (i == 0)
                                CreateVerticalObstacle();
                            CreateBigGap(2);
                            break;
                        case 'E':
                            CreateCoinOnGround(int.Parse(levelLines[i][j + 1].ToString()));
                            break;
                        case 'F':
                            CreateCoinOnAir(int.Parse(levelLines[i][j + 1].ToString()));
                            break;
                        default:
                            CreateFood(GetColorBasedOnChar(levelLines[i][j]), int.Parse(levelLines[i][j + 1].ToString()));
                            break;
                    }
                }

                _currentX += LineX;
            }

            CreateBigGap(3);
            CreateFinishLine();
        }

        private void CreateFinishLine()
        {
            GameObject go = Instantiate(FinishLinePrefab, _transform);
            Vector3 pos = go.transform.position;
            pos.z = _currentZ;
            go.transform.position = pos;
        }

        private void CreateFood(ColorPicker color, int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateGap(1);
                GameObject go = Instantiate(FoodPrefab, _transform);
                Vector3 pos = go.transform.position;
                pos.z = _currentZ;
                pos.x = _currentX;
                go.transform.position = pos;
                go.GetComponent<RunnerFood>().SetColor(color);
            }
        }

        private void CreateCoinOnAir(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateGap(1);
                GameObject go = Instantiate(CoinInAirPrefab, _transform);
                Vector3 pos = go.transform.position;
                pos.z = _currentZ;
                pos.x = _currentX;
                go.transform.position = pos;
            }
        }

        private void CreateCoinOnGround(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateGap(1);
                GameObject go = Instantiate(CoinPrefab, _transform);
                Vector3 pos = go.transform.position;
                pos.z = _currentZ;
                pos.x = _currentX;
                go.transform.position = pos;
            }
        }

        private void CreateBigGap(int count)
        {
            _currentZ += SpaceBeforeAfterObstacles * count;
        }

        private void CreateGap(int count)
        {
            _currentZ += SpaceBetweenEachLine * count;
        }

        private void CreateVerticalObstacle()
        {
            GameObject go = Instantiate(VerticalObstaclePrefab, _transform);
            Vector3 pos = go.transform.position;
            pos.z = _currentZ;
            go.transform.position = pos;
        }

        private void CreateHorizontalObstacle(int lane, bool showThing)
        {
            GameObject go = Instantiate(HorizontalObstaclePrefab, _transform);
            Vector3 pos = go.transform.position;
            pos.x = (-HorizontalObstaclePositionX + lane * HorizontalObstaclePositionX);
            pos.z = _currentZ;
            go.transform.position = pos;

            if (!showThing)
            {
                go.GetComponent<Collider>().enabled = false;
                var renderer = go.GetComponentInChildren<MeshRenderer>();
                if (renderer != null)
                    renderer.enabled = false;
            }
        }

        private void CreateColorChanger(ColorPicker color)
        {
            GameObject go = Instantiate(ColorChangerPrefab, _transform);
            Vector3 pos = go.transform.position;
            pos.z = _currentZ;
            go.transform.position = pos;
            go.GetComponent<RunnerColorChanger>().SetColor(color);
        }

        private static ColorPicker GetColorBasedOnChar(char character)
        {
            switch (character)
            {
                case 'G': return ColorPicker.Green;
                case 'B': return ColorPicker.Blue;
                case 'R': return ColorPicker.Red;
                case 'P': return ColorPicker.Purple;
                default: return ColorPicker.White;
            }
        }
    }
}
