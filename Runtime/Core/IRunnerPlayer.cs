using UnityEngine;

namespace HyperCasualRunner
{
    public enum RunnerFormation
    {
        Horizontal,
        Vertical,
        Straight
    }

    /// <summary>
    /// Implemented by the host game's player character (snake, ball, etc.).
    /// </summary>
    public interface IRunnerPlayer
    {
        Transform Transform { get; }
        float ForwardSpeed { get; set; }
        bool CanMove { get; set; }
        RunnerFormation Formation { get; set; }
        Vector3 LastSectorPosition { get; set; }

        ColorPicker GetColor();
        void SetInitialColor(ColorPicker color);
        void ChangeColor(ColorPicker color);
        float GetHorizontalLimit();

        void Grow();
        void Shrink();
        void RetargetParts();
        void PlayCollectEffect(Vector3 position, ColorPicker color);
        void OnObstacleHit(bool isHead);
        void MoveToCenter(float duration);
        void IncreaseSpeedGradually();
        void DecreaseSpeedGradually();
        void SetFormation(RunnerFormation formation);
    }
}
