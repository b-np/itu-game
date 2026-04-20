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

		public IReadOnlyList<Enemy> Enemies => enemies;

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

		private IEnumerator SpawnEnemy(float delay = 0.5f)
		{
			yield return new WaitForSeconds(delay);
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

			// TODO: better enemy spawning.
			bool addAnother = Random.Range(0, 2) == 0;
			if (addAnother)
			{
				StartCoroutine(SpawnEnemy(2.0f));
			}
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
