using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ITU
{
	public class Battle : MonoBehaviour
	{
		[SerializeField] private Enemy enemyPrefab;
		[SerializeField] private Transform enemyContainer;
		[SerializeField] private float enemySpawnIntervalMin = 0.8f;
		[SerializeField] private float enemySpawnIntervalMax = 1.5f;

		private bool enemySpawnerActive;
		private float enemySpawnTimer;
		private float enemySpawnInterval;
		private readonly List<Enemy> enemies = new List<Enemy>(); //  TODO: use pooling?

		[field: Space, SerializeField] public Player Player { get; private set; }
		[field: SerializeField] public BattleUI UI { get; private set; }

		public IReadOnlyList<Enemy> Enemies => enemies;
		public BattleAnalytics Analytics { get; } = new BattleAnalytics();

		private void Update()
		{
			if (enemySpawnerActive)
			{
				// if possible, do not put this on Update.
				if (enemies.Count == 0)
				{
					NewEnemy();
				}
				else
				{
					enemySpawnTimer += Time.deltaTime;
					if (enemySpawnTimer >= enemySpawnInterval)
					{
						NewEnemy();
					}
				}
			}
		}

		private void NewEnemy()
		{
			enemySpawnTimer = 0.0f;
			enemySpawnInterval = Random.Range(enemySpawnIntervalMin, enemySpawnIntervalMax);
			StartCoroutine(SpawnEnemy());
		}

		public void Prepare()
		{
			Player.Prepare(this);
			Player.OnHit -= OnPlayerHit;
			Player.OnHit += OnPlayerHit;
			while (enemies.Count > 0)
			{
				Destroy(enemies[0].gameObject);
				enemies.RemoveAt(0);
			}
			UI.Prepare(this);
		}

		public void Begin()
		{
			Analytics.Reset();
			StartCoroutine(FreshStart());

			IEnumerator FreshStart()
			{
				yield return SpawnEnemy();
				enemySpawnInterval = Random.Range(enemySpawnIntervalMin, enemySpawnIntervalMax);
				enemySpawnerActive = true;
			}
		}

		private void OnPlayerHit()
		{
			Analytics.ReceivedHit++;
		}

		private IEnumerator SpawnEnemy(float delay = 0.5f)
		{
			yield return new WaitForSeconds(delay);
			Enemy enemy = Instantiate(enemyPrefab, enemyContainer);
			enemy.Initialize(new EnemyStats()
			{
				Attack = 1.0f,
				Health = 1.0f,
				MovementSpeed = Random.Range(0.6f, 1.0f),
			});

			enemy.OnKilled += () =>
			{
				if (enemies.IndexOf(enemy) < 0) return;

				Analytics.TotalKill++;
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
			enemy.Kill();
			Destroy(enemy.gameObject);
		}
	}
}
