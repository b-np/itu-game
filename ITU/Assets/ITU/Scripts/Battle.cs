using System.Collections.Generic;
using UnityEngine;

namespace ITU
{
	public class Battle : MonoBehaviour
	{
		[SerializeField] private Transform enemyContainer;
		[SerializeField] private Enemy enemyPrefab;
		[SerializeField] private float enemySpawnIntervalMin = 5.0f;
		[SerializeField] private float enemySpawnIntervalMax = 15.0f;

		private bool isRunning;
		private float enemySpawnInterval;
		private float enemySpawnElapsed;
		private readonly List<Enemy> enemies = new List<Enemy>(); //  TODO: use pooling?

		[field: Space, SerializeField] public Player Player { get; private set; }
		[field: SerializeField] public BattleUI UI { get; private set; }

		private void Update()
		{
			if (!isRunning) return;

			enemySpawnElapsed += Time.deltaTime;
			if (enemySpawnElapsed >= enemySpawnInterval)
			{
				enemySpawnElapsed = 0.0f;
				SpawnEnemy();
			}
		}

		public void Prepare()
		{
			Player.Prepare();
			while (enemies.Count > 0)
			{
				Destroy(enemies[0].gameObject);
				enemies.RemoveAt(0);
			}

			UI.Prepare(this);
		}

		public void Begin()
		{
			SpawnEnemy();
			isRunning = true;
		}

		public void End()
		{
			isRunning = false;
		}

		public bool TryGetClosestEnemy(out Enemy enemy)
		{
			enemy = null;
			if (enemies.Count == 0) return false;

			float min = float.MaxValue;
			enemy = enemies[0];
			for (int i = 0; i < enemies.Count; i ++)
			{
				float distance = Vector3.Distance(Player.transform.position, enemies[i].transform.position);
				if (distance < min)
				{
					enemy = enemies[i];
					min = distance;
				}
			}

			return min <= Player.Stats.Reach;
		}

		private void SpawnEnemy()
		{
			Enemy enemy = Instantiate(enemyPrefab, enemyContainer);
			enemies.Add(enemy);
			enemySpawnInterval = Random.Range(enemySpawnIntervalMin, enemySpawnIntervalMax);
		}

		// FIXME: should be handled in Battle, this is for testing purposes.
		public void KillEnemy(Enemy enemy)
		{
			int index = enemies.IndexOf(enemy);
			if (index >= 0)
			{
				enemies.RemoveAt(index);
			}
			Destroy(enemy.gameObject);
		}
	}
}
