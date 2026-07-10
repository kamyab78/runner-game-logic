using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HyperCasualRunner.Editor
{
    public static class LevelValidator
    {
        [MenuItem("Tools/Hyper Casual Runner/Validate Levels")]
        public static void Validate()
        {
            string path = "Levels/";
            var settings = Resources.LoadAll<RunnerEngineSettings>("").FirstOrDefault();
            if (settings != null && !string.IsNullOrEmpty(settings.levelResourcePath))
                path = settings.levelResourcePath;

            var levels = Resources.LoadAll<TextAsset>(path)
                .OrderBy(t => int.TryParse(t.name, out var n) ? n : int.MaxValue)
                .ToList();

            if (levels.Count == 0)
            {
                Debug.LogError($"[LevelValidator] No level files found in Resources/{path}.");
                return;
            }

            int total = 0;
            foreach (var lvl in levels)
                total += ValidateOne(lvl.name, lvl.text);

            if (total == 0)
                Debug.Log($"[LevelValidator] OK — all {levels.Count} level(s) parse cleanly.");
            else
                Debug.LogError($"[LevelValidator] {total} problem(s) across {levels.Count} level(s).");
        }

        static int ValidateOne(string name, string text)
        {
            int errors = 0;
            bool anyLine = false;
            var lines = text.Split('\n');

            for (int li = 0; li < lines.Length; li++)
            {
                string trimmed = lines[li].Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                int dash = trimmed.IndexOf('-');
                if (dash < 0)
                {
                    Debug.LogError($"[LevelValidator] {name}: line {li + 1} has no '-' separator: '{trimmed}'");
                    errors++;
                    continue;
                }

                anyLine = true;
                string content = trimmed.Substring(dash + 1);

                if (content.Length % 2 != 0)
                {
                    Debug.LogError($"[LevelValidator] {name}: line {li + 1} content length is odd ({content.Length}).");
                    errors++;
                }

                for (int j = 0; j + 1 < content.Length; j += 2)
                {
                    char c = content[j];
                    char v = content[j + 1];
                    switch (c)
                    {
                        case 'C':
                        case 'A':
                        case 'D':
                            break;
                        default:
                            if (!char.IsDigit(v))
                            {
                                Debug.LogError($"[LevelValidator] {name}: line {li + 1} col {j} — '{c}' needs a digit but got '{v}'.");
                                errors++;
                            }
                            break;
                    }
                }
            }

            if (!anyLine)
            {
                Debug.LogError($"[LevelValidator] {name}: no valid lines.");
                errors++;
            }

            return errors;
        }
    }
}
