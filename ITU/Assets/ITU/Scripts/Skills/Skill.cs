using System;
using UnityEngine;

namespace ITU
{
	public abstract class Skill : ScriptableObject
	{
		[field: SerializeField] public Sprite Icon { get; private set; }

		public event Action Refreshed;

		public void Execute()
		{
			if (!CanExecute()) return;

			OnExecute();
		}

		public void Refresh()
		{
			OnRefresh();
			Refreshed?.Invoke();
		}

		public virtual void Cleanup()
		{
			Refreshed = null;
		}

		protected virtual bool CanExecute() => true;
		protected virtual void OnRefresh() { }

		protected abstract void OnExecute();
	}
}
