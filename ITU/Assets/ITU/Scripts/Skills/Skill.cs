using System;
using UnityEngine;

namespace ITU
{
	public abstract class Skill : ScriptableObject
	{
		private bool isInitialized;

		[field: SerializeField] public Sprite Icon { get; private set; }

		public Battle Battle { get; private set; }
		public Player Owner { get; private set; }

		public event Action OnRefreshed;

		public void Initialize(Battle battle, Player owner)
		{
			Battle = battle;
			Owner = owner;
			OnInitialize();
			isInitialized = true;
		}

		public void Execute()
		{
			if (!isInitialized) return;
			if (!CanExecute()) return;

			OnExecute();
		}

		public void Refresh()
		{
			OnRefresh();
			OnRefreshed?.Invoke();
		}

		public virtual void Cleanup()
		{
			OnRefreshed = null;
		}

		protected virtual void OnInitialize() { }
		protected virtual bool CanExecute() => true;
		protected virtual void OnRefresh() { }

		protected abstract void OnExecute();
	}
}
