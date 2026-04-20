using UnityEngine;

namespace ITU
{
	[RequireComponent(typeof(Collider2D))]
	public class PlayerHitDetector : MonoBehaviour
	{
		[SerializeField] private Player player;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.TryGetComponent<Enemy>(out var Enemy)) return;

			player.Hit();
		}
	}
}
