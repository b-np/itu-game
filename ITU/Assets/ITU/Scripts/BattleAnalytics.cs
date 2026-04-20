using System;

namespace ITU
{
	public class BattleAnalytics
	{
		private int receivedHit;
		private int totalKill;

		public event Action<BattleAnalytics> OnScoreUpdated;

		public int ReceivedHit
		{
			get => receivedHit;
			set
			{
				if (receivedHit == value) return;

				receivedHit = value;
				OnScoreUpdated?.Invoke(this);
			}
		}

		public int TotalKill
		{
			get => totalKill;
			set
			{
				if (totalKill == value) return;

				totalKill = value;
				OnScoreUpdated?.Invoke(this);
			}
		}

		public void Reset()
		{
			ReceivedHit = 0;
			TotalKill = 0;
		}
	}
}
