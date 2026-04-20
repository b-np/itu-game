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
			if (battle.Enemies.Count == 0) return false;

			float min = float.MaxValue;
			enemy = battle.Enemies[0];
			for (int i = 0; i < battle.Enemies.Count; i++)
			{
				float distance = Vector3.Distance(player.transform.position, battle.Enemies[i].transform.position);
				if (distance < min)
				{
					enemy = battle.Enemies[i];
					min = distance;
				}
			}

			return min <= player.Stats.Reach;
		}

		public static Enemy[] GetClosestEnemies(this Battle battle, Player player, int count)
		{
			var list = new List<(float distance, Enemy enemy)>();
			for (int i = 0; i < battle.Enemies.Count; i++)
			{
				float distance = Vector3.Distance(player.transform.position, battle.Enemies[i].transform.position);
				list.Add(new (distance, battle.Enemies[i]));
			}
			list.Sort((l, r) =>
			{
				if (l.distance < r.distance) return -1;
				if (l.distance > r.distance) return 1;
				return 0;
			});
			return list.Take(count).Select(i => i.enemy).ToArray();
		}
	}
}
