using System;
using UnityEngine;

namespace ITU
{
	public class Enemy : MonoBehaviour, IHittable
	{
		[SerializeField] private float speed = 1.0f;

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

		public void Hit(float value)
		{
			OnHit?.Invoke();
		}

		public void Kill()
		{
			OnKilled?.Invoke();
		}

		private void UpdateMovement()
		{
			float value = Time.deltaTime * speed;
			transform.position += -1 * value * transform.right; // -1 because we're moving left.
		}
	}
}
