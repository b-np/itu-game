using UnityEngine;
using UnityEngine.UI;

namespace ITU
{
	public class BattleUI : MonoBehaviour
	{
		private Battle battle;

		[SerializeField] private Button[] skillButtons;

		public void Prepare(Battle battle)
		{
			this.battle = battle;
			for (int i = 0; i < skillButtons.Length; i++)
			{
				int index = i;
				Button skillButton = skillButtons[i];

				skillButton.onClick.RemoveAllListeners();
				if (index < battle.Player.Skills.Length)
				{
					Skill skill = battle.Player.Skills[index];
					skill.Refreshed += () =>
					{
						skillButton.interactable = true;
					};

					skillButton.onClick.AddListener(() =>
					{
						skill.Execute();
						skillButton.interactable = false;
					});
					skillButton.image.sprite = skill.Icon;

					skillButton.interactable = true;
					skillButton.gameObject.SetActive(true);
				}
				else
				{
					skillButton.interactable = false;
					skillButton.gameObject.SetActive(false);
				}
			}
		}
	}
}
