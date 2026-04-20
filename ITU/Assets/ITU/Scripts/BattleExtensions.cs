using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ITU
{
	public static class BattleExtensions
	{
		public static bool TryGetClosestEnemy(this Battle battle, Player player, out Enemy enemy)
		{
			enemy = null;
			IReadOnlyList<Enemy> enemies = battle.EnemyManager.Enemies;
			if (enemies.Count == 0) return false;

			float min = float.MaxValue;
			enemy = enemies[0];
			for (int i = 0; i < enemies.Count; i++)
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

		public static Enemy[] GetClosestEnemies(this Battle battle, Player player, int count, float maxReach = 0.0f)
		{
			IReadOnlyList<Enemy> enemies = battle.EnemyManager.Enemies;
			var list = new List<(float distance, Enemy enemy)>();
			for (int i = 0; i < enemies.Count; i++)
			{
				float distance = Vector3.Distance(player.transform.position, enemies[i].transform.position);
				if (maxReach > 0.0f && distance > maxReach) continue;

				list.Add(new (distance, enemies[i]));
			}
			list.Sort((l, r) =>
			{
				if (l.distance < r.distance) return -1;
				if (l.distance > r.distance) return 1;
				return 0;
			});
			return list.Take(count)
				.Select(i => i.enemy)
				.ToArray();
		}
	}
}
