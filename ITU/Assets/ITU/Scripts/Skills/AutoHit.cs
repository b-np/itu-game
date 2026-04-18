using UnityEngine;

namespace ITU
{
	[CreateAssetMenu(fileName = "Hit", menuName = "ITU/Skills/Create Auto Hit")]
	public class AutoHit : Hit
	{
		protected override void OnRefresh()
		{
			base.OnRefresh();
			Execute();
		}
	}
}
