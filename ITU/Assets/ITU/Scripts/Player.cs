using System;
using UnityEngine;

namespace ITU
{
	public class Player : MonoBehaviour
	{
		[field: SerializeField] public PlayerStats Stats { get; private set; } = new PlayerStats();
		[field: SerializeField] public Skill[] Skills { get; private set; }

		public event Action OnHit;

		private void Update()
		{
			float deltaTime = Time.deltaTime;
			for (int i = 0; i < Skills.Length; i++)
			{
				if (Skills[i] is ITickable tickable)
				{
					tickable.Tick(deltaTime);
				}
			}
		}

		public void Prepare(Battle battle)
		{
			foreach (var skill in Skills)
			{
				skill.Initialize(battle, this);
				skill.Cleanup();
				skill.Refresh();
			}
		}

		public void Hit()
		{
			OnHit?.Invoke();
		}
	}
}
