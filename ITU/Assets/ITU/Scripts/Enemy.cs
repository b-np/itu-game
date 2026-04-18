using System;
using UnityEngine;

namespace ITU
{
	public class Enemy : MonoBehaviour, IHittable
	{
		[SerializeField] private float speed = 1.0f;
		[SerializeField] private float lifetime = 10f; // FIXME: should not have this.

		private float elapsedLifetime = 0.0f;

		public event Action OnHit;

		private void Update()
		{
			UpdateMovement();
			elapsedLifetime += Time.deltaTime;
			if (elapsedLifetime >= lifetime)
			{
				Game.Instance.Battle.KillEnemy(this);
			}
		}

		private void OnDestroy()
		{
			OnHit = null;
		}

		public void Hit(float value)
		{
			Game.Instance.Battle.KillEnemy(this);
			OnHit?.Invoke();
		}

		private void UpdateMovement()
		{
			float value = Time.deltaTime * speed;
			transform.position += -1 * value * transform.right; // -1 because we're moving left.
		}
	}
}
