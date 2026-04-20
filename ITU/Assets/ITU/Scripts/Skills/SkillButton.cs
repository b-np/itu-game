using UnityEngine;
using UnityEngine.UI;

namespace ITU
{
	[RequireComponent(typeof(Button))]
	public class SkillButton : MonoBehaviour
	{
		[field: SerializeField] public Button Button { get; private set; }
		[field: SerializeField] public Image Icon { get; private set; }

		private void OnEnable()
		{
			if (Button == null)
			{
				Button = GetComponent<Button>();
			}
		}
	}
}
