using UnityEngine;

namespace ITU
{
	[CreateAssetMenu(fileName = nameof(Attack), menuName = "ITU/Skills/Create " + nameof(Attack))]
	public class Attack : Skill, ITickable, IHasCooldown
	{
		private bool canExecute = true;
		private bool cooldown = false;

		[field: SerializeField] public float Cooldown { get; private set; } = 1.0f;
		public float ElapsedCooldown { get; private set; }

		public void Tick(float deltaTime)
		{
			if (cooldown)
			{
				ElapsedCooldown += deltaTime;
				if (ElapsedCooldown >= Cooldown)
				{
					Refresh();
				}
			}
		}

		protected override bool CanExecute() => canExecute;

		protected override void OnRefresh()
		{
			base.OnRefresh();
			ElapsedCooldown = 0.0f;
			canExecute = true;
			cooldown = false;
		}

		protected override void OnExecute()
		{
			if (Battle.TryGetClosestEnemy(Owner, out Enemy target))
			{
				float value = Owner.Stats.Attack;
				if (target is IHittable hittable)
				{
					hittable.Hit(value);
				}
			}

			ElapsedCooldown = 0.0f;
			canExecute = false;
			cooldown = true;
		}
	}
}
