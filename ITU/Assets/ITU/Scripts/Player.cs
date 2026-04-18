using UnityEngine;

namespace ITU
{
	public class Player : MonoBehaviour
	{
		[field: SerializeField] public PlayerStats Stats { get; private set; } = new PlayerStats();

		[Header("Skills")]
		[field: SerializeField] public AutoHit AutoHit { get; private set; }
		[field: SerializeField] public Skill[] Skills { get; private set; }

		private void Update()
		{
			float deltaTime = Time.deltaTime;
			AutoHit.Tick(deltaTime);
			for (int i = 0; i < Skills.Length; i++)
			{
				if (Skills[i] is ITickable tickable)
				{
					tickable.Tick(deltaTime);
				}
			}
		}

		public void Prepare()
		{
			AutoHit.Cleanup();
			AutoHit.Refresh();
			foreach (var skill in Skills)
			{
				skill.Cleanup();
				skill.Refresh();
			}
		}
	}
}
