using UnityEngine;

namespace ITU
{
	public class Game : SingletonBehaviour<Game>
	{
		[field: SerializeField] public Battle Battle { get; private set; }

		protected override void OnInitialize()
		{
			base.OnInitialize();
		}

		private void Start()
		{
			Battle.Prepare();
			Battle.Begin();
		}
	}
}
