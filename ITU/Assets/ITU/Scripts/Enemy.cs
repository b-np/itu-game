using System;
using UnityEngine;

namespace ITU
{
	public class Enemy : MonoBehaviour, IHittable
	{
		public EnemyStats Stats { get; private set; } = new EnemyStats();
		public Health Health { get; private set; } 

		public event Action OnHit;
		public event Action OnKilled;

		private void Update()
		{
			UpdateMovement();
		}

		private void OnDestroy()
		{
			OnHit = null;
		}

		public void Initialize(EnemyStats stats)
		{
			Stats = stats;
			Health = new Health(Stats.Health);
		}

		public void Hit(float value)
		{
			Health.Decrease(value);
			Debug.Log($"{Health.Current}/{Health.Max} -- {Health.IsAlive}");
			OnHit?.Invoke();
			if (!Health.IsAlive)
			{
				Kill();
			}
		}

		public void Kill()
		{
			OnKilled?.Invoke();
		}

		private void UpdateMovement()
		{
			if (!Health.IsAlive) return;

			float value = Time.deltaTime * Stats.MovementSpeed;
			transform.position += -1 * value * transform.right; // -1 because we're moving left.
		}
	}
}
