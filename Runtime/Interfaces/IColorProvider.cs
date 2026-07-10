using UnityEngine;

namespace HyperCasualRunner
{
    public interface IColorProvider
    {
        Color GetColor(ColorPicker color);
    }
}
