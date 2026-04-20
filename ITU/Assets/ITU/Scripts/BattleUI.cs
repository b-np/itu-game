using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITU
{
	public class BattleUI : MonoBehaviour
	{
		public Battle Battle { get; private set; }

		[SerializeField] private TMP_Text scoreText;
		[SerializeField] private SkillButton[] skillButtons;

		public void Prepare(Battle battle)
		{
			scoreText.text = "00";
			Battle = battle;
			for (int i = 0; i < skillButtons.Length; i++)
			{
				int index = i;
				SkillButton skillButton = skillButtons[i];

				skillButton.Button.onClick.RemoveAllListeners();
				if (index < Battle.Player.Skills.Length)
				{
					Skill skill = Battle.Player.Skills[index];
					if (skill is IHasCooldown cooldown)
					{
						cooldown.Cooldown.OnRefreshed += () =>
						{
							skillButton.Button.interactable = true;
						};
					}

					skillButton.Button.onClick.AddListener(() =>
					{
						skill.Execute();
						skillButton.Button.interactable = false;
					});
					skillButton.Icon.sprite = skill.Icon;

					skillButton.Button.interactable = true;
					skillButton.gameObject.SetActive(true);
				}
				else
				{
					skillButton.Button.interactable = false;
					skillButton.gameObject.SetActive(false);
				}
			}
			Battle.Analytics.OnScoreUpdated -= OnScoreUpdated;
			Battle.Analytics.OnScoreUpdated += OnScoreUpdated;
		}

		private void OnScoreUpdated(BattleAnalytics data)
		{
			int count = Mathf.Max(data.TotalKill - data.ReceivedHit);
			scoreText.text = $"{count:00}";
		}
	}
}
