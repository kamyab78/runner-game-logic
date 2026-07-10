using DG.Tweening;
using UnityEngine;

namespace HyperCasualRunner
{
    public class RunnerMotor : MonoBehaviour
    {
        private float _horizontalLimit;

        public void Initialize(float horizontalLimit)
        {
            _horizontalLimit = horizontalLimit;
        }

        public void MoveForward(Transform tr, float speed)
        {
            tr.position += Vector3.forward * (speed * Time.deltaTime);
        }

        public void MoveHorizontal(Transform tr, Vector3 dir, float speed)
        {
            tr.position += dir * (speed * Time.deltaTime);

            if (tr.position.x >= _horizontalLimit)
            {
                tr.position = new Vector3(_horizontalLimit, tr.position.y, tr.position.z);
            }
            else if (tr.position.x <= -_horizontalLimit)
            {
                tr.position = new Vector3(-_horizontalLimit, tr.position.y, tr.position.z);
            }
        }

        public void MoveHorizontalCenter(Transform tr, float time)
        {
            tr.DOMoveX(0, time);
        }
    }
}
