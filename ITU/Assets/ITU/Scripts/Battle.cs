using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ITU
{
	public class Battle : MonoBehaviour
	{
		[SerializeField] private Enemy enemyPrefab;
		[SerializeField] private Transform enemyContainer;

		private readonly List<Enemy> enemies = new List<Enemy>(); //  TODO: use pooling?

		[field: Space, SerializeField] public Player Player { get; private set; }
		[field: SerializeField] public BattleUI UI { get; private set; }

		public void Prepare()
		{
			Player.Prepare(this);
			while (enemies.Count > 0)
			{
				Destroy(enemies[0].gameObject);
				enemies.RemoveAt(0);
			}

			UI.Prepare(this);
		}

		public void Begin()
		{
			StartCoroutine(SpawnEnemy());
		}

		public void End()
		{
		}

		public bool TryGetClosestEnemy(Player player, out Enemy enemy)
		{
			enemy = null;
			if (enemies.Count == 0) return false;

			float min = float.MaxValue;
			enemy = enemies[0];
			for (int i = 0; i < enemies.Count; i ++)
			{
				float distance = Vector3.Distance(player.transform.position, enemies[i].transform.position);
				if (distance < min)
				{
					enemy = enemies[i];
					min = distance;
				}
			}

			return min <= player.Stats.Reach;
		}

		private IEnumerator SpawnEnemy()
		{
			yield return new WaitForSeconds(0.5f);
			Enemy enemy = Instantiate(enemyPrefab, enemyContainer);

			// TODO: review this later.
			// KillEnemy should not be on both OnHit and OnKilled?
			enemy.OnHit += () =>
			{
				KillEnemy(enemy);
			};
			enemy.OnKilled += () => 
			{
				if (enemies.IndexOf(enemy) >= 0) KillEnemy(enemy);
				if (enemies.Count == 0)
				{
					StartCoroutine(SpawnEnemy());
				}
			};

			enemies.Add(enemy);
		}

		private void KillEnemy(Enemy enemy)
		{
			int index = enemies.IndexOf(enemy);
			if (index >= 0)
			{
				enemies.RemoveAt(index);
			}
			enemy.Kill();
			Destroy(enemy.gameObject);
		}
	}
}
