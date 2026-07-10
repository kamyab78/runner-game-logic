using UnityEngine;

namespace HyperCasualRunner
{
    /// <summary>
    /// Runtime service locator set by the host game during scene bootstrap.
    /// </summary>
    public static class RunnerContext
    {
        public static RunnerGameController GameController { get; set; }
        public static IRunnerPlayer Player { get; set; }
        public static RunnerEvents Events { get; set; }
        public static RunnerEngineSettings Settings { get; set; }
        public static IColorProvider ColorProvider { get; set; }
        public static ILevelProgressProvider LevelProgress { get; set; }
        public static IVibrationProvider Vibration { get; set; } = new NullVibrationProvider();
        public static ITutorialGate TutorialGate { get; set; } = new NullTutorialGate();
        public static RunnerCameraController Camera { get; set; }
        public static RunnerFinishLine FinishLine { get; set; }
    }
}
