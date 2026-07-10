using UnityEngine;

namespace HyperCasualRunner
{
    public interface IChangeColor
    {
        void ChangeColor(Color color);
        void PlayChangeColorParticle(GameObject particlePrefab, Color color);
    }
}
