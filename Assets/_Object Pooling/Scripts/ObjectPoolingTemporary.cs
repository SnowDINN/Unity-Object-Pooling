using System;
using UnityEngine;

namespace Anonymous.Pooling
{
	public class ObjectPoolingTemporary : MonoBehaviour
	{
		private const float rate = 0.1f;

		[SerializeField] [NotEditableField] private float time;
		private Action onDestory;
		private float removeWaitTime;

		public void Enable(float removeWaitTime)
		{
			this.removeWaitTime = removeWaitTime;
		}

		public void Disable(Action onDestory)
		{
			this.onDestory = onDestory;
			InvokeRepeating(nameof(invokeAsync), 0.0f, rate);
		}

		private void invokeAsync()
		{
			time += rate;

			if (gameObject.activeSelf)
			{
				CancelInvoke(nameof(invokeAsync));
				time = 0;
			}

			if (removeWaitTime < time)
			{
				onDestory.Invoke();

				CancelInvoke(nameof(invokeAsync));
				Destroy(gameObject);
			}
		}
	}
}