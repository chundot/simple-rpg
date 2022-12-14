using RPG.Core.UI.Tooltips;
using UnityEngine;

namespace RPG.UI.Quests
{
  public class QuestTooltipSpawner : TooltipSpawner
  {
    public override bool CanCreateTooltip => true;

    public override void UpdateTooltip(GameObject tooltip)
    {
      var quest = GetComponent<QuestItemUI>().Status;
      tooltip.GetComponent<QuestTooltipUI>().Setup(quest);
    }
  }

}