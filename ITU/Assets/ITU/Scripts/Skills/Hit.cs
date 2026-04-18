using UnityEngine;

namespace ITU
{
	[CreateAssetMenu(fileName = "Hit", menuName = "ITU/Skills/Create Hit")]
	public class Hit : Skill, ITickable, IHasCooldown
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
			Battle battle = Game.Instance.Battle;
			if (battle.TryGetClosestEnemy(out Enemy target))
			{
				float value = battle.Player.Stats.Attack;
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
