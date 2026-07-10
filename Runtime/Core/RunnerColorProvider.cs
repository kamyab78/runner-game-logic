using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerColorProvider : MonoBehaviour, IColorProvider
    {
        [SerializeField] private Color _white;
        [SerializeField] private Color _green;
        [SerializeField] private Color _red;
        [SerializeField] private Color _purple;
        [SerializeField] private Color _blue;

        protected virtual void Awake()
        {
            RunnerContext.ColorProvider = this;
        }

        public Color GetColor(ColorPicker color)
        {
            switch (color)
            {
                case ColorPicker.Green: return _green;
                case ColorPicker.Red: return _red;
                case ColorPicker.Purple: return _purple;
                case ColorPicker.Blue: return _blue;
                default: return _white;
            }
        }
    }
}
