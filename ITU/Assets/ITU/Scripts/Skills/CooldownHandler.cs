using System;
using UnityEngine;

namespace ITU
{
	[Serializable]
	public class CooldownHandler : ITickable
	{
		[field: SerializeField] public float Time { get; private set; } = 1.0f;

		public bool InCooldown { get; private set; }
		public float ElapsedTime { get; private set; }
		public float RemainingTime => Math.Min(Time - ElapsedTime, 0.0f);

		public event Action OnRefreshed;

		public void Cleanup()
		{
			OnRefreshed = null;
		}

		public void Tick(float deltaTime)
		{
			if (!InCooldown) return;

			ElapsedTime += deltaTime;
			if (ElapsedTime >= Time)
			{
				Refresh();
			}
		}

		public void Trigger()
		{
			ElapsedTime = 0.0f;
			InCooldown = true;
		}

		public void Refresh()
		{
			ElapsedTime = Time;
			InCooldown = false;
			OnRefreshed?.Invoke();
		}
	}
}
