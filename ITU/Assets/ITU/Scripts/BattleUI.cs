using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITU
{
	public class BattleUI : MonoBehaviour
	{
		public Battle Battle { get; private set; }

		[SerializeField] private TMP_Text scoreText;
		[SerializeField] private Button[] skillButtons;

		public void Prepare(Battle battle)
		{
			scoreText.text = "00";
			Battle = battle;
			for (int i = 0; i < skillButtons.Length; i++)
			{
				int index = i;
				Button skillButton = skillButtons[i];

				skillButton.onClick.RemoveAllListeners();
				if (index < Battle.Player.Skills.Length)
				{
					Skill skill = Battle.Player.Skills[index];
					if (skill is IHasCooldown cooldown)
					{
						cooldown.Cooldown.OnRefreshed += () =>
						{
							skillButton.interactable = true;
						};
					}

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
