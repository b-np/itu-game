using UnityEngine;

namespace ITU
{
	public class Battle : MonoBehaviour
	{
		[field: SerializeField] public EnemyManager EnemyManager { get; private set; }
		[field: SerializeField] public Player Player { get; private set; }
		[field: SerializeField] public BattleUI UI { get; private set; }

		public BattleAnalytics Analytics { get; } = new BattleAnalytics();

		public void Prepare()
		{
			Player.Prepare(this);
			Player.OnHit -= OnPlayerHit;
			Player.OnHit += OnPlayerHit;
			EnemyManager.Prepare();
			EnemyManager.OnEnemyKilled += OnEnemyKilled;

			UI.Prepare(this);
		}

		public void Begin()
		{
			Analytics.Reset();
			StartCoroutine(EnemyManager.SpawnInitialEnemies());
		}

		private void OnEnemyKilled()
		{
			Analytics.TotalKill++;
		}

		private void OnPlayerHit()
		{
			Analytics.ReceivedHit++;
		}
	}
}
