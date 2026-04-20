using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace ITU
{
	[CreateAssetMenu(fileName = nameof(AreaAttack), menuName = "ITU/Skills/Create " + nameof(AreaAttack))]
	public class AreaAttack : Skill, ITickable, IHasCooldown
	{
		[field: SerializeField] public CooldownHandler Cooldown { get; private set; } = new CooldownHandler();

		public void Tick(float deltaTime)
		{
			Cooldown.Tick(deltaTime);
		}

		public override void Cleanup()
		{
			base.Cleanup();
			Cooldown.Cleanup();
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
			Cooldown.OnRefreshed -= OnRefresh;
			Cooldown.OnRefreshed += OnRefresh;
		}

		protected override bool CanExecute() => !Cooldown.InCooldown;

		protected override void OnExecute()
		{
			Enemy[] enemies = Battle.GetClosestEnemies(Owner, 2, Owner.Stats.Reach * 2.0f);
			foreach (var enemy in enemies)
			{
				enemy.Hit(Owner.Stats.Attack);
			}
			Cooldown.Trigger();
		}
	}
}
