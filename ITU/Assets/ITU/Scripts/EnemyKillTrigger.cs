using UnityEngine;

namespace ITU
{
	[RequireComponent(typeof(Collider2D))]
	public class EnemyKillTrigger : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent<Enemy>(out var enemy))
			{
				enemy.Kill();
			}
		}
	}
}
