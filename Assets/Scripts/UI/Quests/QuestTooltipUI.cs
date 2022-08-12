using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
  public class QuestTooltipUI : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI _title, _rewards;
    [SerializeField] Transform _objectives;
    [SerializeField] GameObject _objectivePrefab, _objectiveIncompletePrefab;
    const string NO_REWARD = "没有任何奖励";
    public void Setup(QuestStatus status)
    {
      _title.text = status.Quest.Title;
      _objectives.DestroyAllChildren();
      foreach (var objective in status.Quest.Objectives)
      {
        var objectiveInstance = Instantiate(status.IsObjectiveCompleted(objective.Reference) ? _objectivePrefab : _objectiveIncompletePrefab, _objectives);
        objectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective.Description;
      }
      _rewards.text = GetRewardText(status.Quest.Rewards);
    }
    public string GetRewardText(IEnumerable<Quest.Reward> rewards) => rewards.Count() == 0 ? NO_REWARD : rewards.Aggregate(new StringBuilder(), (b, r) => b.Append(", " + r.Item.DisplayName).Append(r.Num > 1 ? $" x {r.Num}" : ""), (b) => b.Remove(0, 2).ToString());
  }
}