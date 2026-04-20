using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace ITU
{
	public class EnemyManager : MonoBehaviour
	{
		[SerializeField] private Enemy enemyPrefab;
		[SerializeField] private Transform enemySpawnPoint;
		[SerializeField] private Transform enemyContainer;
		[SerializeField] private float enemySpawnIntervalMin = 0.8f;
		[SerializeField] private float enemySpawnIntervalMax = 1.5f;

		private bool enemySpawnerActive;
		private float enemySpawnTimer;
		private float enemySpawnInterval;
		private readonly List<Enemy> enemies = new List<Enemy>(); //  TODO: use pooling?

		public IReadOnlyList<Enemy> Enemies => enemies;

		public event System.Action OnEnemyKilled;

		private void Update()
		{
			enemySpawnTimer += Time.deltaTime;
			if (enemySpawnTimer >= enemySpawnInterval)
			{
				NewEnemy();
			}
		}

		private void OnDestroy()
		{
			OnEnemyKilled = null;
		}

		public void Prepare()
		{
			while (enemies.Count > 0)
			{
				Destroy(enemies[0].gameObject);
				enemies.RemoveAt(0);
			}
		}

		public IEnumerator SpawnInitialEnemies()
		{
			yield return SpawnEnemy();
			enemySpawnInterval = Random.Range(enemySpawnIntervalMin, enemySpawnIntervalMax);
		}

		private void NewEnemy()
		{
			enemySpawnTimer = 0.0f;
			enemySpawnInterval = Random.Range(enemySpawnIntervalMin, enemySpawnIntervalMax);
			StartCoroutine(SpawnEnemy());
		}

		private IEnumerator SpawnEnemy(float delay = 0.5f)
		{
			yield return new WaitForSeconds(delay);
			Enemy enemy = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity, enemyContainer);
			enemy.Initialize(new EnemyStats()
			{
				Attack = 1.0f,
				Health = 1.0f,
				MovementSpeed = Random.Range(0.6f, 1.0f),
			});

			enemy.OnKilled += () =>
			{
				if (enemies.IndexOf(enemy) < 0) return;
				KillEnemy(enemy);
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
			OnEnemyKilled?.Invoke();
			Destroy(enemy.gameObject);
		}
	}
}
