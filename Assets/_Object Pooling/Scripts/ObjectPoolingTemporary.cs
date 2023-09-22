using UnityEngine;

namespace Anonymous.Pooling
{
    public class ObjectPoolingTemporary : MonoBehaviour
    {
        [SerializeField] [NotEditableField] private float time;
        private float removeWaitTime;

        public void Enable(float removeWaitTime)
        {
            this.removeWaitTime = removeWaitTime;
        }

        public void Disable()
        {
            InvokeRepeating(nameof(invokeAsync), 0.0f, 0.1f);
        }

        private void invokeAsync()
        {
            if (gameObject.activeSelf)
            {
                CancelInvoke(nameof(invokeAsync));
                time = 0;
            }

            if (removeWaitTime < time)
            {
                CancelInvoke(nameof(invokeAsync));
                Destroy(gameObject);
            }

            time += 0.1f;
        }
    }
}